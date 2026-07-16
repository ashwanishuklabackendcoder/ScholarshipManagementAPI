using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class StudentProgramApplicationService : IStudentProgramApplicationService
    {
        private readonly AppDbContext _context;

        public StudentProgramApplicationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CandidateProgramResponseDto>> GetCandidateProgramsAsync(long studentId)
        {
            var student = await _context.StudentRegistrations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.IsActive);

            if (student == null)
            {
                throw new CustomException("Student registration not found.");
            }

            var programs = await _context.KfPrograms
                .AsNoTracking()
                .Include(p => p.University)
                .Include(p => p.Faculty)
                .Include(p => p.KfProgramDocuments)
                    .ThenInclude(pd => pd.DocumentType)
                .Where(p => p.IsActive)
                .ToListAsync();

            var list = new List<CandidateProgramResponseDto>();
            foreach (var p in programs)
            {
                list.Add(new CandidateProgramResponseDto
                {
                    ProgramId = p.ProgramId,
                    ProgramName = p.ProgramName,
                    ProgramCode = p.ProgramCode,
                    UniversityName = p.University.UniversityName,
                    FacultyName = p.Faculty.FacultyName,
                    RequiredDocuments = p.KfProgramDocuments.Select(pd => new RequiredDocumentDto
                    {
                        ProgramDocumentId = pd.ProgramDocumentId,
                        DocumentTypeId = pd.DocumentTypeId,
                        DocumentTypeName = pd.DocumentType.DocumentName,
                        IsRequired = pd.IsRequired
                    }).ToList()
                });
            }

            return list;
        }

        public async Task<long> ApplyAsync(long studentId, ApplyRequestDto dto, long userId)
        {
            var student = await _context.StudentRegistrations
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.IsActive);

            if (student == null)
            {
                throw new CustomException("Student registration not found.");
            }

            var program = await _context.KfPrograms
                .FirstOrDefaultAsync(x => x.ProgramId == dto.ProgramId && x.IsActive);

            if (program == null)
            {
                throw new CustomException("Program not found or is inactive.");
            }

            // Check for existing active application (statuses 1 to 7 are active application phases)
            var activeStatuses = new[] { 1, 2, 3, 5, 7 }; // Draft, AcceptanceInProcess, Sponsored, Awarded, Registered
            var hasActiveApp = await _context.StudentProgramApplications
                .AnyAsync(x => x.StudentId == studentId && activeStatuses.Contains(x.ApplicationStatus));

            if (hasActiveApp)
            {
                throw new CustomException("Student already has an active program application.");
            }

            var application = new StudentProgramApplication
            {
                StudentId = studentId,
                ProgramId = dto.ProgramId,
                ApplicationStatus = (int)StudentApplicationStatus.Draft,
                AppliedDate = DateTime.UtcNow,
                Remarks = dto.Remarks,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };

            _context.StudentProgramApplications.Add(application);
            await _context.SaveChangesAsync();

            // Create History Log
            var history = new StudentHistory
            {
                StudentId = studentId,
                ApplicationId = application.ApplicationId,
                Title = "Application Draft Created",
                Description = $"Created draft application for program {program.ProgramName} (Code: {program.ProgramCode}).",
                HistoryType = 1, // HistoryType: System or Workflow Action
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.StudentHistories.Add(history);
            await _context.SaveChangesAsync();

            return application.ApplicationId;
        }

        public async Task<bool> CancelApplicationAsync(long applicationId, long userId)
        {
            var app = await _context.StudentProgramApplications
                .Include(a => a.Program)
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (app == null)
            {
                throw new CustomException("Application not found.");
            }

            if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
            {
                throw new CustomException("Cancellation is only allowed while the application is in Draft status.");
            }

            // Record History (ON DELETE SET NULL will set History's ApplicationId to NULL)
            var history = new StudentHistory
            {
                StudentId = app.StudentId,
                ApplicationId = null,
                Title = "Application Draft Cancelled",
                Description = $"Cancelled and deleted the draft application for program {app.Program.ProgramName}.",
                HistoryType = 2, // Cancellation
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.StudentHistories.Add(history);

            // Fetch and delete files physically
            var docs = await _context.StudentProgramDocuments
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();

            foreach (var doc in docs)
            {
                if (File.Exists(doc.StoragePath))
                {
                    try { File.Delete(doc.StoragePath); } catch { /* Ignore */ }
                }
            }

            // Remove database entries (Cascade will handle StudentProgramDocuments)
            _context.StudentProgramApplications.Remove(app);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SubmitApplicationAsync(long applicationId, long userId)
        {
            var app = await _context.StudentProgramApplications
                .Include(a => a.Program)
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (app == null)
            {
                throw new CustomException("Application not found.");
            }

            if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
            {
                throw new CustomException("Only Draft applications can be submitted.");
            }

            app.ApplicationStatus = (int)StudentApplicationStatus.AcceptanceInProcess;
            app.SubmittedDate = DateTime.UtcNow;
            app.UpdatedBy = userId;
            app.UpdatedDate = DateTime.UtcNow;

            var history = new StudentHistory
            {
                StudentId = app.StudentId,
                ApplicationId = app.ApplicationId,
                Title = "Application Submitted",
                Description = $"Submitted application for program {app.Program.ProgramName}. Status changed to Acceptance In Process.",
                HistoryType = 3, // Submission
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.StudentHistories.Add(history);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentProgramApplicationResponseDto?> GetApplicationAsync(long applicationId)
        {
            var app = await _context.StudentProgramApplications
                .AsNoTracking()
                .Include(a => a.Program)
                .Include(a => a.StudentProgramDocuments)
                    .ThenInclude(d => d.DocumentType)
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (app == null) return null;

            return new StudentProgramApplicationResponseDto
            {
                ApplicationId = app.ApplicationId,
                StudentId = app.StudentId,
                ProgramId = app.ProgramId,
                ProgramName = app.Program.ProgramName,
                ProgramCode = app.Program.ProgramCode,
                ApplicationStatus = app.ApplicationStatus,
                ApplicationStatusName = Enum.GetName(typeof(StudentApplicationStatus), app.ApplicationStatus) ?? app.ApplicationStatus.ToString(),
                AppliedDate = app.AppliedDate,
                SubmittedDate = app.SubmittedDate,
                Remarks = app.Remarks,
                CreatedBy = app.CreatedBy,
                CreatedDate = app.CreatedDate,
                Documents = app.StudentProgramDocuments.Select(d => new StudentProgramDocumentResponseDto
                {
                    StudentProgramDocumentId = d.StudentProgramDocumentId,
                    ApplicationId = d.ApplicationId,
                    ProgramDocumentId = d.ProgramDocumentId,
                    DocumentTypeId = d.DocumentTypeId,
                    DocumentTypeName = d.DocumentType.DocumentName,
                    OriginalFileName = d.OriginalFileName,
                    StoredFileName = d.StoredFileName,
                    StoragePath = d.StoragePath,
                    ContentType = d.ContentType,
                    FileSize = d.FileSize,
                    ReviewerRemark = d.ReviewerRemark,
                    UploadedBy = d.UploadedBy,
                    UploadedDate = d.UploadedDate
                }).ToList()
            };
        }

        public async Task<StudentProgramDocumentResponseDto> UploadDocumentAsync(long applicationId, long programDocumentId, long documentTypeId, IFormFile file, long userId)
        {
            var app = await _context.StudentProgramApplications
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (app == null)
            {
                throw new CustomException("Application not found.");
            }

            if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
            {
                throw new CustomException("Uploading documents is only allowed in Draft status.");
            }

            if (file == null || file.Length == 0)
            {
                throw new CustomException("Uploaded file is empty.");
            }

            var cleanName = Path.GetFileName(file.FileName).Replace(" ", "_");
            var storedName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}_{cleanName}";
            var directory = Path.Combine("wwwroot", "uploads", "student-applications", applicationId.ToString());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fullPath = Path.Combine(directory, storedName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var doc = new StudentProgramDocument
            {
                ApplicationId = applicationId,
                ProgramDocumentId = programDocumentId,
                DocumentTypeId = documentTypeId,
                OriginalFileName = file.FileName,
                StoredFileName = storedName,
                StoragePath = fullPath,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedBy = userId,
                UploadedDate = DateTime.UtcNow
            };

            _context.StudentProgramDocuments.Add(doc);

            // History Log
            var docType = await _context.KfDocumentTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DocumentTypeId == documentTypeId);

            var history = new StudentHistory
            {
                StudentId = app.StudentId,
                ApplicationId = app.ApplicationId,
                Title = "Document Uploaded",
                Description = $"Uploaded document: {file.FileName} for document type {(docType != null ? docType.DocumentName : documentTypeId.ToString())}.",
                HistoryType = 4, // Document Action
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.StudentHistories.Add(history);

            await _context.SaveChangesAsync();

            return new StudentProgramDocumentResponseDto
            {
                StudentProgramDocumentId = doc.StudentProgramDocumentId,
                ApplicationId = doc.ApplicationId,
                ProgramDocumentId = doc.ProgramDocumentId,
                DocumentTypeId = doc.DocumentTypeId,
                DocumentTypeName = docType?.DocumentName ?? documentTypeId.ToString(),
                OriginalFileName = doc.OriginalFileName,
                StoredFileName = doc.StoredFileName,
                StoragePath = doc.StoragePath,
                ContentType = doc.ContentType,
                FileSize = doc.FileSize,
                UploadedBy = doc.UploadedBy,
                UploadedDate = doc.UploadedDate
            };
        }

        public async Task<bool> DeleteDocumentAsync(long applicationId, long documentId, long userId)
        {
            var doc = await _context.StudentProgramDocuments
                .Include(d => d.DocumentType)
                .FirstOrDefaultAsync(x => x.StudentProgramDocumentId == documentId && x.ApplicationId == applicationId);

            if (doc == null)
            {
                throw new CustomException("Document not found.");
            }

            var app = await _context.StudentProgramApplications
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (app == null)
            {
                throw new CustomException("Application not found.");
            }

            if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
            {
                throw new CustomException("Only Draft application documents can be deleted.");
            }

            if (File.Exists(doc.StoragePath))
            {
                try { File.Delete(doc.StoragePath); } catch { /* Ignore */ }
            }

            _context.StudentProgramDocuments.Remove(doc);

            var history = new StudentHistory
            {
                StudentId = app.StudentId,
                ApplicationId = app.ApplicationId,
                Title = "Document Deleted",
                Description = $"Deleted document: {doc.OriginalFileName} of type {doc.DocumentType.DocumentName}.",
                HistoryType = 5, // Document Action
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.StudentHistories.Add(history);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentProgramDocumentResponseDto>> GetDocumentsAsync(long applicationId)
        {
            return await _context.StudentProgramDocuments
                .AsNoTracking()
                .Include(d => d.DocumentType)
                .Where(x => x.ApplicationId == applicationId)
                .Select(d => new StudentProgramDocumentResponseDto
                {
                    StudentProgramDocumentId = d.StudentProgramDocumentId,
                    ApplicationId = d.ApplicationId,
                    ProgramDocumentId = d.ProgramDocumentId,
                    DocumentTypeId = d.DocumentTypeId,
                    DocumentTypeName = d.DocumentType.DocumentName,
                    OriginalFileName = d.OriginalFileName,
                    StoredFileName = d.StoredFileName,
                    StoragePath = d.StoragePath,
                    ContentType = d.ContentType,
                    FileSize = d.FileSize,
                    ReviewerRemark = d.ReviewerRemark,
                    UploadedBy = d.UploadedBy,
                    UploadedDate = d.UploadedDate
                })
                .ToListAsync();
        }

        public async Task<List<StudentHistoryResponseDto>> GetHistoryAsync(long studentId)
        {
            return await _context.StudentHistories
                .AsNoTracking()
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new StudentHistoryResponseDto
                {
                    StudentHistoryId = x.StudentHistoryId,
                    StudentId = x.StudentId,
                    ApplicationId = x.ApplicationId,
                    Title = x.Title,
                    Description = x.Description,
                    HistoryType = x.HistoryType,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }
    }
}
