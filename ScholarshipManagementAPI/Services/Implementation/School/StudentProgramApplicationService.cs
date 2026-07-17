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
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate student
                var student = await _context.StudentRegistrations
                    .FirstOrDefaultAsync(x => x.StudentId == studentId && x.IsActive);

                if (student == null)
                {
                    throw new CustomException("Student registration not found.");
                }

                // Validate selected program
                var program = await _context.KfPrograms
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.ProgramId == dto.ProgramId &&
                        x.AccreditationStatus == (int)AccreditationStatusEnum.Accredited &&
                        x.IsActive);

                if (program == null)
                {
                    throw new CustomException("Selected program does not exist or is inactive.");
                }

                // TODO:
                // Validate that this program is actually available for this student.
                // (Candidate program/business eligibility validation goes here.)

                // Check whether student already has an active application
                var activeStatuses = new[]
                {
                    (int)StudentApplicationStatus.Draft,
                    (int)StudentApplicationStatus.AcceptanceInProcess,
                    (int)StudentApplicationStatus.Sponsored,
                    (int)StudentApplicationStatus.Awarded,
                    (int)StudentApplicationStatus.Registered
                };

                bool hasActiveApplication = await _context.StudentProgramApplications
                    .AnyAsync(x =>
                        x.StudentId == studentId &&
                        activeStatuses.Contains(x.ApplicationStatus));

                if (hasActiveApplication)
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

                await AddHistoryAsync(
                    studentId: application.StudentId,
                    applicationId: application.ApplicationId,
                    title: "Application Draft Created",
                    description: $"Created draft application for program '{program.ProgramName}' ({program.ProgramCode}).",
                    historyType: StudentHistoryTypeEnum.ApplicationDraftCreated,
                    userId: userId);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return application.ApplicationId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CancelApplicationAsync(long applicationId, long userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
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
                    throw new CustomException("Only Draft applications can be cancelled.");
                }

                // Delete uploaded files
                var documents = await _context.StudentProgramDocuments
                    .Where(x => x.ApplicationId == applicationId)
                    .ToListAsync();

                foreach (var document in documents)
                {
                    if (File.Exists(document.StoragePath))
                    {
                        try
                        {
                            File.Delete(document.StoragePath);
                        }
                        catch
                        {
                            // Optional: Log exception
                        }
                    }
                }

                // History
                await AddHistoryAsync(
                    studentId: app.StudentId,
                    applicationId: null,
                    title: "Application Draft Cancelled",
                    description: $"Cancelled draft application for program '{app.Program.ProgramName}'.",
                    historyType: StudentHistoryTypeEnum.ApplicationDraftCancelled,
                    userId: userId);

                // Delete application (documents will be deleted by cascade)
                _context.StudentProgramApplications.Remove(app);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> SubmitApplicationAsync(long applicationId, long userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var app = await _context.StudentProgramApplications
                    .Include(a => a.Program)
                        .ThenInclude(p => p.KfProgramDocuments)
                    .Include(a => a.StudentProgramDocuments)
                    .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

                if (app == null)
                {
                    throw new CustomException("Application not found.");
                }

                if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
                {
                    throw new CustomException("Only Draft applications can be submitted.");
                }

                // Validate required documents
                var requiredDocuments = app.Program.KfProgramDocuments
                    .Where(x => x.IsRequired)
                    .Select(x => x.ProgramDocumentId)
                    .ToList();

                var uploadedDocuments = app.StudentProgramDocuments
                    .Select(x => x.ProgramDocumentId)
                    .ToList();

                var missingDocuments = requiredDocuments
                    .Except(uploadedDocuments)
                    .ToList();

                if (missingDocuments.Any())
                {
                    throw new CustomException("Please upload all required documents before submitting the application.");
                }

                app.ApplicationStatus = (int)StudentApplicationStatus.AcceptanceInProcess;
                app.SubmittedDate = DateTime.UtcNow;
                app.UpdatedBy = userId;
                app.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await AddHistoryAsync(
                    studentId: app.StudentId,
                    applicationId: app.ApplicationId,
                    title: "Application Submitted",
                    description: $"Submitted application for program '{app.Program.ProgramName}'. Status changed to Acceptance In Process.",
                    historyType: StudentHistoryTypeEnum.ApplicationSubmitted,
                    userId: userId);

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<StudentProgramApplicationResponseDto?> GetApplicationAsync(long applicationId)
        {
            var app = await _context.StudentProgramApplications
                .AsNoTracking()
                .Include(a => a.Program)
                    .ThenInclude(p => p.University)
                .Include(a => a.Program)
                    .ThenInclude(p => p.Faculty)
                .Include(a => a.Program)
                    .ThenInclude(p => p.KfProgramDocuments)
                        .ThenInclude(pd => pd.DocumentType)
                .Include(a => a.StudentProgramDocuments)
                    .ThenInclude(d => d.DocumentType)
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (app == null)
            {
                return null;
            }

            return new StudentProgramApplicationResponseDto
            {
                ApplicationId = app.ApplicationId,
                StudentId = app.StudentId,

                ProgramId = app.ProgramId,
                ProgramName = app.Program.ProgramName,
                ProgramCode = app.Program.ProgramCode,

                UniversityName = app.Program.University.UniversityName,
                FacultyName = app.Program.Faculty.FacultyName,

                ApplicationStatus = app.ApplicationStatus,
                ApplicationStatusName = Enum.GetName(typeof(StudentApplicationStatus), app.ApplicationStatus)
                                        ?? app.ApplicationStatus.ToString(),

                AppliedDate = app.AppliedDate,
                SubmittedDate = app.SubmittedDate,

                Remarks = app.Remarks,

                CreatedBy = app.CreatedBy,
                CreatedDate = app.CreatedDate,

                RequiredDocuments = app.Program.KfProgramDocuments
                    .Select(pd => new RequiredDocumentDto
                    {
                        ProgramDocumentId = pd.ProgramDocumentId,
                        DocumentTypeId = pd.DocumentTypeId,
                        DocumentTypeName = pd.DocumentType.DocumentName,
                        IsRequired = pd.IsRequired
                    })
                    .ToList(),

                Documents = app.StudentProgramDocuments
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
                        UploadedDate = d.UploadedDate,

                        IsRequired = app.Program.KfProgramDocuments
                            .Any(pd =>
                                pd.ProgramDocumentId == d.ProgramDocumentId &&
                                pd.IsRequired)
                    })
                    .ToList()
            };
        }


        public async Task<StudentProgramDocumentResponseDto> UploadDocumentAsync(long applicationId, long programDocumentId,
            long documentTypeId, IFormFile file, long userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate Application
                var app = await _context.StudentProgramApplications
                    .Include(x => x.Program)
                    .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

                if (app == null)
                {
                    throw new CustomException("Application not found.");
                }

                // Only Draft applications allow document upload
                if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
                {
                    throw new CustomException("Documents can only be uploaded while the application is in Draft status.");
                }

                // Validate File
                if (file == null || file.Length == 0)
                {
                    throw new CustomException("Please select a valid document.");
                }

                // Validate Program Document
                var programDocument = await _context.KfProgramDocuments
                    .Include(x => x.DocumentType)
                    .FirstOrDefaultAsync(x =>
                        x.ProgramDocumentId == programDocumentId &&
                        x.ProgramId == app.ProgramId);

                if (programDocument == null)
                {
                    throw new CustomException("Invalid program document.");
                }

                // Validate Document Type
                if (programDocument.DocumentTypeId != documentTypeId)
                {
                    throw new CustomException("Invalid document type.");
                }

                // Prevent Duplicate Upload
                bool alreadyUploaded = await _context.StudentProgramDocuments
                    .AnyAsync(x =>
                        x.ApplicationId == applicationId &&
                        x.ProgramDocumentId == programDocumentId);

                if (alreadyUploaded)
                {
                    throw new CustomException(
                        $"{programDocument.DocumentType.DocumentName} has already been uploaded.");
                }

                // Create Upload Folder
                var folder = Path.Combine(
                    "wwwroot",
                    "uploads",
                    "student-applications",
                    applicationId.ToString());

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Generate File Name
                var extension = Path.GetExtension(file.FileName);

                var storedFileName =
                    $"{Guid.NewGuid():N}{extension}";

                var filePath = Path.Combine(folder, storedFileName);

                // Save File
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = new StudentProgramDocument
                {
                    ApplicationId = applicationId,
                    ProgramDocumentId = programDocumentId,
                    DocumentTypeId = documentTypeId,

                    OriginalFileName = file.FileName,
                    StoredFileName = storedFileName,
                    StoragePath = filePath,

                    ContentType = file.ContentType,
                    FileSize = file.Length,

                    UploadedBy = userId,
                    UploadedDate = DateTime.UtcNow
                };

                _context.StudentProgramDocuments.Add(document);

                await AddHistoryAsync(
                    studentId: app.StudentId,
                    applicationId: app.ApplicationId,
                    title: "Document Uploaded",
                    description: $"Uploaded '{programDocument.DocumentType.DocumentName}'.",
                    historyType: StudentHistoryTypeEnum.DocumentUploaded,
                    userId: userId);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new StudentProgramDocumentResponseDto
                {
                    StudentProgramDocumentId = document.StudentProgramDocumentId,
                    ApplicationId = document.ApplicationId,

                    ProgramDocumentId = document.ProgramDocumentId,

                    DocumentTypeId = document.DocumentTypeId,
                    DocumentTypeName = programDocument.DocumentType.DocumentName,

                    OriginalFileName = document.OriginalFileName,
                    StoredFileName = document.StoredFileName,
                    StoragePath = document.StoragePath,

                    ContentType = document.ContentType,
                    FileSize = document.FileSize,

                    UploadedBy = document.UploadedBy,
                    UploadedDate = document.UploadedDate,

                    IsRequired = programDocument.IsRequired
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteDocumentAsync(long applicationId, long documentId, long userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var app = await _context.StudentProgramApplications
                    .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

                if (app == null)
                {
                    throw new CustomException("Application not found.");
                }

                if (app.ApplicationStatus != (int)StudentApplicationStatus.Draft)
                {
                    throw new CustomException("Documents can only be deleted while the application is in Draft status.");
                }

                var document = await _context.StudentProgramDocuments
                    .Include(x => x.DocumentType)
                    .FirstOrDefaultAsync(x =>
                        x.StudentProgramDocumentId == documentId &&
                        x.ApplicationId == applicationId);

                if (document == null)
                {
                    throw new CustomException("Document not found.");
                }

                // Delete physical file
                if (File.Exists(document.StoragePath))
                {
                    try
                    {
                        File.Delete(document.StoragePath);
                    }
                    catch
                    {
                        // Optional: Log exception
                    }
                }

                _context.StudentProgramDocuments.Remove(document);

                await AddHistoryAsync(
                    studentId: app.StudentId,
                    applicationId: app.ApplicationId,
                    title: "Document Deleted",
                    description: $"Deleted '{document.DocumentType.DocumentName}'.",
                    historyType: StudentHistoryTypeEnum.DocumentDeleted,
                    userId: userId);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<StudentProgramDocumentResponseDto>> GetDocumentsAsync(long applicationId)
        {
            var application = await _context.StudentProgramApplications
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

            if (application == null)
            {
                throw new CustomException("Application not found.");
            }

            return await _context.StudentProgramDocuments
                .AsNoTracking()
                .Include(d => d.DocumentType)
                .Include(d => d.ProgramDocument)
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
                    UploadedDate = d.UploadedDate,

                    IsRequired = d.ProgramDocument.IsRequired
                })
                .ToListAsync();
        }


        public async Task<List<StudentHistoryResponseDto>> GetHistoryAsync(long studentId)
        {
            var studentExists = await _context.StudentRegistrations
                .AsNoTracking()
                .AnyAsync(x => x.StudentId == studentId && x.IsActive);

            if (!studentExists)
            {
                throw new CustomException("Student not found.");
            }

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


        private Task AddHistoryAsync( long studentId, long? applicationId, string title,
            string description, StudentHistoryTypeEnum historyType, long userId)
        {
            _context.StudentHistories.Add(new StudentHistory
            {
                StudentId = studentId,
                ApplicationId = applicationId,
                Title = title,
                Description = description,
                HistoryType = (int)historyType,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow
            });

            return Task.CompletedTask;
        }


    }

}
