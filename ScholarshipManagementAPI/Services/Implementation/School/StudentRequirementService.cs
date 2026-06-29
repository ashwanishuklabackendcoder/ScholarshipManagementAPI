using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;
using ScholarshipManagementAPI.Services.Interface.School;
using System.Text.RegularExpressions;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class StudentRequirementService : IStudentRequirementService
    {
        private readonly AppDbContext _context;
        private readonly ILocalFileService _localFileService;
        public StudentRequirementService(AppDbContext context , ILocalFileService localFileService)
        {
            _context = context;
            _localFileService = localFileService;
        }


        // ---------------- StudentRequirementMap ----------------
        //public async Task<long> CreateStudentRequirementMapAsync(StudentRequirementMappingDto dto)
        //{
        //    if (dto.StudentID <= 0)
        //        throw new CustomException("Invalid student.");

        //    if (dto.ReqId <= 0)
        //        throw new CustomException("Invalid requirement.");

        //    // Check: Student already has a request
        //    var exists = await _context.StudentReqLists
        //        .Include(x => x.Req)
        //        .ThenInclude(r => r.Course)
        //        .AnyAsync(x => x.StudentId == dto.StudentID && x.Req.Course.UniversityId == dto.UniversityId);

        //    if (exists)
        //    {
        //        throw new CustomException("Student already applied in this university.");
        //    }

        //    var entity = new StudentReqList
        //    {
        //        StudentId = dto.StudentID,
        //        ReqId = dto.ReqId,
        //        CreatedBy = dto.CreatedBy,
        //        CreatedDate = dto.CreatedDate
        //    };

        //    _context.StudentReqLists.Add(entity);
        //    await _context.SaveChangesAsync();

        //    // Get the generated StudentReqId
        //    var studentReqId = entity.StudentReqId;


        //    // Attach uploaded docs & update StudentDocuments with this studentReqId
        //    var tempDocs = await _context.StudentDocuments
        //        .Where(x => x.StudentReqId == null && x.StudentId == dto.StudentID)
        //        .ToListAsync();

        //    foreach (var doc in tempDocs)
        //    {
        //        doc.StudentReqId = studentReqId;
        //    }

        //    await _context.SaveChangesAsync();


        //    return entity.StudentReqId;
        //}



        public async Task<long> CreateStudentRequirementMapAsync(StudentRequirementMappingDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (dto.StudentID <= 0)
                    throw new CustomException("Invalid student.");

                if (dto.ReqId <= 0)
                    throw new CustomException("Invalid requirement.");

                var actualUniversityId = await _context.UnCourseReqs
                    .Where(r => r.ReqId == dto.ReqId)
                    .Select(r => r.Course.UniversityId)
                    .FirstOrDefaultAsync();

                if (actualUniversityId == 0)
                    throw new CustomException("Invalid requirement mapping.");

                if (dto.UniversityId != actualUniversityId)
                    throw new CustomException("Requirement does not belong to selected university.");

                // 🔹 Check existing
                var exists = await _context.StudentReqLists
                    .Include(x => x.Req)
                    .ThenInclude(r => r.Course)
                    .AnyAsync(x => x.StudentId == dto.StudentID 
                                   && x.Req.Course.UniversityId == dto.UniversityId);

                if (exists)
                    throw new CustomException("Student already applied in this university.");

                // 🔹 Create mappingD
                var entity = new StudentReqList
                {
                    StudentId = dto.StudentID,
                    ReqId = dto.ReqId,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = dto.CreatedDate
                };

                _context.StudentReqLists.Add(entity);
                await _context.SaveChangesAsync();

                var studentReqId = entity.StudentReqId;

                // 🔹 Attach uploaded docs (IMPORTANT FIX BELOW 👇)
                //var tempDocs = await _context.StudentDocuments
                //    .Where(x => x.StudentReqId == null
                //             && x.UploadSessionId == dto.UploadSessionId) // use session, NOT studentId
                //    .ToListAsync();


                var tempDocs = await _context.StudentDocuments
                    .Where(x => x.StudentReqId == null
                    && x.UploadSessionId == dto.UploadSessionId
                    && x.StudentId == dto.StudentID)
                    .ToListAsync();

                foreach (var doc in tempDocs)
                {
                    doc.StudentReqId = studentReqId;
                }

                await _context.SaveChangesAsync();

                // 🔹 Commit
                await transaction.CommitAsync();

                return studentReqId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateStudentRequirementMapAsync(StudentRequirementMappingDto dto)
        {
            var entity = await _context.StudentReqLists
                .FirstOrDefaultAsync(x => x.StudentReqId == dto.StudentReqID);

            if (entity == null)
                throw new CustomException("Record not found.");

            // Optional: prevent duplicate again
            var exists = await _context.StudentReqLists
                .Include(x => x.Req)
                .ThenInclude(r => r.Course)
                .AnyAsync(x => x.StudentId == dto.StudentID 
                               && x.Req.Course.UniversityId == dto.UniversityId 
                               && x.StudentReqId != dto.StudentReqID);

            if (exists)
                throw new CustomException("Student already applied in this university.");

            // studentId is not updatable, 
            // entity.StudentId = dto.StudentID;
            entity.ReqId = dto.ReqId;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateStudentRequirementMapByUniversityAsync(StudentRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            var entity = await _context.StudentReqLists
                .FirstOrDefaultAsync(x => x.StudentReqId == dto.StudentReqID);

            if (entity == null)
                return false;

            // Optional: prevent duplicate again
            var exists = await _context.StudentReqLists
                .Include(x => x.Req)
                .ThenInclude(r => r.Course)
                .AnyAsync(x => x.StudentId == dto.StudentID 
                               && x.Req.Course.UniversityId == dto.UniversityId 
                               && x.StudentReqId != dto.StudentReqID);

            if (exists)
                throw new CustomException("Student already applied in this university.");

            // entity.StudentId = dto.StudentID;
            // entity.ReqId = dto.ReqId;

            // Validate documents FIRST (if accepting)
            if (dto.DocumentStatus == (int)DocumentStatus.Accepted)
            {
                var missingDocs = await ValidateRequiredDocumentsAsync(entity.StudentReqId);

                if (missingDocs.Any())
                    throw new CustomException("All required documents must be uploaded before accepting");
            }

            // Validate workflow BEFORE updating anything

            ValidateUniversityAndNgoWorkflow(entity, dto);


            // Now apply updates
            entity.DocumentStatus = dto.DocumentStatus;

            if (dto.DocumentStatus == (int)DocumentStatus.Accepted)
            {
                // Accepted
                entity.SemesterStartDate = dto.SemesterStartDate;
                entity.LetterAccepCode = dto.LetterAccepCode;
                entity.UniAwardingstatus = dto.UniAwardingStatus;

                // reset others
                entity.ReasonRejection = null;
                entity.MissedDocuments = null;
                entity.ReasonInProgress = null;
            }
            else if (dto.DocumentStatus == (int)DocumentStatus.Rejected)
            {
                // Rejected
                entity.ReasonRejection = dto.ReasonRejection;
                entity.MissedDocuments = dto.MissedDocuments;

                // reset others
                entity.SemesterStartDate = null;
                entity.LetterAccepCode = null;
                entity.UniAwardingstatus = null;
                entity.ReasonInProgress = null;
            }
            else if (dto.DocumentStatus == (int)DocumentStatus.InProcess)
            {
                // In Process
                entity.ReasonInProgress = dto.ReasonInProgress;

                // reset others
                entity.SemesterStartDate = null;
                entity.LetterAccepCode = null;
                entity.UniAwardingstatus = null;
                entity.ReasonRejection = null;
                entity.MissedDocuments = null;
            }

            entity.UniStatusBy = currentUser.LoginId;
            entity.UniStatusDate = DateTime.UtcNow;


            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateStudentRequirementMapByNgoAsync(StudentRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            var entity = await _context.StudentReqLists
                .FirstOrDefaultAsync(x => x.StudentReqId == dto.StudentReqID);

            if (entity == null)
                return false;

            // Optional: prevent duplicate again
            var exists = await _context.StudentReqLists
                .Include(x => x.Req)
                .ThenInclude(r => r.Course)
                .AnyAsync(x => x.StudentId == dto.StudentID 
                               && x.Req.Course.UniversityId == dto.UniversityId 
                               && x.StudentReqId != dto.StudentReqID);

            if (exists)
                throw new CustomException("Student already applied in this university.");

            // entity.StudentId = dto.StudentID;
            // entity.ReqId = dto.ReqId;

            // Validate workflow FIRST
            ValidateUniversityAndNgoWorkflow(entity, dto);

            // NGO can only act AFTER awarding
            if (entity.UniAwardingstatus != (int)AwardingStatus.Awarded)
            {
                throw new CustomException("Student must be awarded before NGO action");
            }


            // Apply NGO updates
            entity.DaAdmissionStatus = dto.DaAdmissionStatus;
            entity.TotalCost = dto.TotalCost;
            entity.UniAwardingstatusCost = dto.UniAwardingStatusCost;
            entity.DonorId = dto.DonorId;

            entity.DaStatusBy = currentUser.LoginId;
            entity.DaStatusDate = DateTime.UtcNow;


            await _context.SaveChangesAsync();

            return true;
        }




        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entity = await _context.StudentReqLists
                    .FirstOrDefaultAsync(x => x.StudentReqId == id);

                if (entity == null)
                    return false;

                // Delete child documents FIRST
                var documents = await _context.StudentDocuments
                    .Where(x => x.StudentReqId == id)
                    .ToListAsync();

                if (documents.Any())
                {
                    _context.StudentDocuments.RemoveRange(documents);
                }

                // Delete parent
                _context.StudentReqLists.Remove(entity);

                await _context.SaveChangesAsync();

                // Commit
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                // Rollback on error
                await transaction.RollbackAsync();
                throw;
            }
        }



        // ---------------- GET BY ID ----------------
        public async Task<StudentRequirementRequestDto?> GetByIdAsync(long id)
        {
            return await _context.StudentReqLists
                .Include(x => x.Donor)
                .Include(x => x.Student)
                .Include(x => x.Req)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.CourseType)
                .ThenInclude(x => x.University)
                .AsNoTracking()
                .Where(x => x.StudentReqId == id)
                .Select(x => new StudentRequirementRequestDto
                {
                    StudentReqID = x.StudentReqId,
                    StudentID = x.StudentId,
                    ReqId = x.ReqId,
                    DocumentStatus = x.DocumentStatus,
                    ReasonRejection = x.ReasonRejection,
                    MissedDocuments = x.MissedDocuments,
                    SemesterStartDate = x.SemesterStartDate,
                    LetterAccepCode = x.LetterAccepCode,
                    UniStatusBy = x.UniStatusBy,
                    UniStatusDate = x.UniStatusDate,
                    DaAdmissionStatus = x.DaAdmissionStatus,
                    DaStatusBy = x.DaStatusBy,
                    DaStatusDate = x.DaStatusDate,
                    DonorId = x.DonorId,
                    TotalCost = x.TotalCost,
                    CreateEmailBy = x.CreateEmailBy,
                    ReasonInProgress = x.ReasonInProgress,
                    UniAwardingStatus = x.UniAwardingstatus,
                    UniAwardingStatusCost = x.UniAwardingstatusCost,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    AcademicYear = x.Req != null ? x.Req.AcademicYear : null,
                    StudentFullName = x.Student != null ? $"{x.Student.StudentSalutation} {x.Student.StudentFirstName} {x.Student.StudentLastName}" : null,
                    StudentNumber = x.Student != null ? x.Student.StudentNumber : null,
                    StudentPhoto = x.Student != null ? x.Student.Photo : null,
                    CourseId = x.Req != null && x.Req.Course != null ? x.Req.Course.CourseId : (long?)null,
                    CourseName = x.Req != null && x.Req.Course != null ? x.Req.Course.CourseName : null,
                    CourseTypeId = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null ? x.Req.Course.CourseType.CourseTypeId : (long?)null,
                    CourseTypeName = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null ? x.Req.Course.CourseType.CourseTypeName : null,
                    UniversityId = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null && x.Req.Course.CourseType.University != null ? x.Req.Course.CourseType.University.UniversityId : (long?)null,
                    UniversityName = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null && x.Req.Course.CourseType.University != null ? x.Req.Course.CourseType.University.UniversityName : null,
                    RequiredDocuments = x.Req != null ? x.Req.RequiredDocuments : null,
                    DonorName = x.Donor != null ? x.Donor.DonorName : null
                })
                .FirstOrDefaultAsync();
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<StudentRequirementRequestDto>> GetByFilterAsync(StudentRequirementFilterDto filter)
        {
            var query = _context.StudentReqLists
                .Include(x => x.Donor)
                .Include(x => x.Student)
                .Include(x => x.Req)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.CourseType)
                .ThenInclude(x => x.University)
                .AsNoTracking()
                .AsQueryable();

            // Student filter
            if (filter.StudentID.HasValue)
            {
                query = query.Where(x => x.StudentId == filter.StudentID.Value);
            }

            // Request filter
            if (filter.ReqId.HasValue)
            {
                query = query.Where(x => x.ReqId == filter.ReqId.Value);
            }

            // University Admission Status
            if (filter.DocumentStatus.HasValue)
            {
                query = query.Where(x => x.DocumentStatus == filter.DocumentStatus.Value);
            }

            // DA Admission Status
            if (filter.DaAdmissionStatus.HasValue)
            {
                query = query.Where(x => x.DaAdmissionStatus == filter.DaAdmissionStatus.Value);
            }

            // Awarding Status
            if (filter.UniAwardingStatus.HasValue)
            {
                query = query.Where(x => x.UniAwardingstatus == filter.UniAwardingStatus.Value);
            }

            // Donor filter
            if (filter.DonorId.HasValue)
            {
                query = query.Where(x => x.DonorId == filter.DonorId.Value);
            }


            // Created Date range
            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                var from = filter.CreatedFrom ?? DateTime.MinValue;
                var to = filter.CreatedTo ?? DateTime.MaxValue;

                query = query.Where(x =>
                    x.CreatedDate >= from &&
                    x.CreatedDate <= to
                );
            }

            // Semester Start Date range
            if (filter.SemesterStartFrom.HasValue || filter.SemesterStartTo.HasValue)
            {
                var from = filter.SemesterStartFrom ?? DateTime.MinValue;
                var to = filter.SemesterStartTo ?? DateTime.MaxValue;

                query = query.Where(x =>
                    x.SemesterStartDate.HasValue &&
                    x.SemesterStartDate.Value >= from &&
                    x.SemesterStartDate.Value <= to
                );
            }


            // Cost
            if (filter.MinTotalCost.HasValue)
            {
                query = query.Where(x => x.TotalCost.HasValue && x.TotalCost.Value >= filter.MinTotalCost.Value);
            }

            if (filter.MaxTotalCost.HasValue)
            {
                query = query.Where(x => x.TotalCost.HasValue && x.TotalCost.Value <= filter.MaxTotalCost.Value);
            }



            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.Student != null &&
                    (
                        x.Student.StudentNumber.ToLower().Contains(search) ||
                        x.Student.StudentFirstName.ToLower().Contains(search) ||
                        (x.Student.StudentLastName != null && x.Student.StudentLastName.ToLower().Contains(search))
                    )
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.StudentReqId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new StudentRequirementRequestDto
                {
                    StudentReqID = x.StudentReqId,
                    StudentID = x.StudentId,
                    ReqId = x.ReqId,
                    DocumentStatus = x.DocumentStatus,
                    ReasonRejection = x.ReasonRejection,
                    MissedDocuments = x.MissedDocuments,
                    SemesterStartDate = x.SemesterStartDate,
                    LetterAccepCode = x.LetterAccepCode,
                    UniStatusBy = x.UniStatusBy,
                    UniStatusDate = x.UniStatusDate,
                    DaAdmissionStatus = x.DaAdmissionStatus,
                    DaStatusBy = x.DaStatusBy,
                    DaStatusDate = x.DaStatusDate,
                    DonorId = x.DonorId,
                    TotalCost = x.TotalCost,
                    CreateEmailBy = x.CreateEmailBy,
                    ReasonInProgress = x.ReasonInProgress,
                    UniAwardingStatus = x.UniAwardingstatus,
                    UniAwardingStatusCost = x.UniAwardingstatusCost,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    AcademicYear = x.Req != null ? x.Req.AcademicYear : null,
                    StudentFullName = x.Student != null ? $"{x.Student.StudentSalutation} {x.Student.StudentFirstName} {x.Student.StudentLastName}" : null,
                    StudentNumber = x.Student != null ? x.Student.StudentNumber : null,
                    StudentPhoto = x.Student != null ? x.Student.Photo : null,
                    CourseId = x.Req != null && x.Req.Course != null ? x.Req.Course.CourseId : (long?)null,
                    CourseName = x.Req != null && x.Req.Course != null ? x.Req.Course.CourseName : null,
                    CourseTypeId = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null ? x.Req.Course.CourseType.CourseTypeId : (long?)null,
                    CourseTypeName = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null ? x.Req.Course.CourseType.CourseTypeName : null,
                    UniversityId = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null && x.Req.Course.CourseType.University != null ? x.Req.Course.CourseType.University.UniversityId : (long?)null,
                    UniversityName = x.Req != null && x.Req.Course != null && x.Req.Course.CourseType != null && x.Req.Course.CourseType.University != null ? x.Req.Course.CourseType.University.UniversityName : null,
                    RequiredDocuments = x.Req != null ? x.Req.RequiredDocuments : null,
                    DonorName = x.Donor != null ? x.Donor.DonorName : null
                })
                .ToListAsync();

            return new PagedResultDto<StudentRequirementRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }





        public async Task<string> UploadAsync(long studentReqId, long masterDocId, IFormFile file)
        {
            // 🔹 1. Validate file
            if (file == null || file.Length == 0)
                throw new Exception("File required");

            var extension = Path.GetExtension(file.FileName)?.ToLower();

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type");

            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("File size must be less than 5MB");

            // 🔹 2. Validate student requirement
            var studentReq = await _context.StudentReqLists
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.StudentReqId == studentReqId);

            if (studentReq == null)
                throw new Exception("Invalid requirement");

            // 🔹 3. Validate master document exists
            var masterDoc = await _context.UnMasterDocs
                .FirstOrDefaultAsync(x => x.UniversityDocsId == masterDocId);

            if (masterDoc == null)
                throw new Exception("Invalid document type");

            // 🔹 4. Validate document belongs to requirement
            var req = await _context.UnCourseReqs
                .FirstOrDefaultAsync(x => x.ReqId == studentReq.ReqId);

            if (req == null || string.IsNullOrEmpty(req.RequiredDocuments))
                throw new Exception("Requirement document configuration missing");

            var requiredDocs = req.RequiredDocuments
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            if (!requiredDocs.Contains((int)masterDocId))
                throw new Exception("Document not required for this requirement");

            // 🔹 5. Check existing document (BEFORE upload)
            var existing = await _context.StudentDocuments
                .FirstOrDefaultAsync(x =>
                    x.StudentReqId == studentReqId &&
                    x.MasterDocId == masterDocId);

            var cleanDocName = SanitizeFileName(masterDoc.DocumentName);

            // 🔹 6. Generate file key
            var fileKey = $"students/{studentReq.Student.StudentNumber}/requirement/{studentReqId}/{masterDocId}_{cleanDocName}/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            // 🔹 7. Upload file
            await _localFileService.UploadAsync(stream, fileKey);

            // later delete old file if exists (optional)

            // 🔹 8. Update or insert DB
            if (existing != null)
            {
                // optional: delete old file
                // await _localFileService.DeleteAsync(existing.FileUrlName);

                existing.FileUrlName = fileKey;
                existing.CreatedDate = DateTime.UtcNow;
            }
            else
            {
                _context.StudentDocuments.Add(new StudentDocument
                {
                    StudentId = studentReq.StudentId,
                    StudentReqId = null,
                    MasterDocId = masterDocId,
                    FileUrlName = fileKey,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                });
            }

            // 🔹 9. Save DB
            await _context.SaveChangesAsync();

            // 🔹 10. Return full URL
            return _localFileService.GetFileUrl(fileKey);
        }


        public async Task<string> UploadAsync(UploadDocumentRequestDto request)
        {
            // 🔹 1. Validate file
            if (request.File == null || request.File.Length == 0)
                throw new CustomException("File required");

            var extension = Path.GetExtension(request.File.FileName)?.ToLower();
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                throw new CustomException("Invalid file type");

            if (request.File.Length > 5 * 1024 * 1024)
                throw new CustomException("File size must be less than 5MB");


            // 🔹 2. Validate student
            var student = await _context.StudentData
                .FirstOrDefaultAsync(x => x.StudentId == request.StudentId);

            if (student == null)
                throw new CustomException("Invalid student");


            // 🔹 3. Validate master document
            var masterDoc = await _context.UnMasterDocs
                .FirstOrDefaultAsync(x => x.UniversityDocsId == request.MasterDocId);

            if (masterDoc == null)
                throw new CustomException("Invalid document type");

            // null before save
            long? studentReqId = null;

            // 🔹 4. OPTIONAL: Validate requirement (only if mapped)
            if (request.StudentReqId != null && request.StudentReqId >0)
            {
                studentReqId = request.StudentReqId;
                var studentReq = await _context.StudentReqLists
                    .FirstOrDefaultAsync(x => x.StudentReqId == request.StudentReqId);

                if (studentReq == null)
                    throw new CustomException("Invalid requirement");

                var req = await _context.UnCourseReqs
                    .FirstOrDefaultAsync(x => x.ReqId == studentReq.ReqId);

                if (req != null && !string.IsNullOrEmpty(req.RequiredDocuments))
                {
                    var requiredDocs = req.RequiredDocuments
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();

                    if (!requiredDocs.Contains((int)request.MasterDocId))
                        throw new CustomException("Document not required for this requirement");
                }
            }

            // 🔹 5. Check existing document (Replace logic)
            var existing = await _context.StudentDocuments
                .FirstOrDefaultAsync(x =>
                    x.MasterDocId == request.MasterDocId &&
                    (
                        (request.StudentReqId != null && x.StudentReqId == request.StudentReqId) ||
                        (request.StudentReqId == null && x.UploadSessionId == request.UploadSessionId)
                    ));

            var cleanDocName = SanitizeFileName(masterDoc.DocumentName);

            // 🔹 6. Generate file key
            var fileKey = $"students/{student.StudentNumber}/documents/{request.MasterDocId}_{cleanDocName}/{Guid.NewGuid()}{extension}";

            using var stream = request.File.OpenReadStream();

            // 🔹 7. Upload file
            await _localFileService.UploadAsync(stream, fileKey);

            // 🔹 8. Insert / Update DB
            if (existing != null)
            {
                // optional: delete old file
                // await _localFileService.DeleteAsync(existing.FileUrlName);

                existing.FileUrlName = fileKey;
                existing.CreatedDate = DateTime.UtcNow;
            }
            else
            {
                _context.StudentDocuments.Add(new StudentDocument
                {
                    StudentId = request.StudentId,
                    StudentReqId = studentReqId, 
                    UploadSessionId = request.UploadSessionId,
                    MasterDocId = request.MasterDocId,
                    FileUrlName = fileKey,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                }); ;
            }

            // 🔹 9. Save DB
            await _context.SaveChangesAsync();

            // 🔹 10. Return URL
            return _localFileService.GetFileUrl(fileKey);
        }





        public async Task<List<StudentDocumentDto>> GetDocumentStatusAsync(long studentReqId)
        {
            // 🔹 1. Get student requirement
            var studentReq = await _context.StudentReqLists
                .FirstOrDefaultAsync(x => x.StudentReqId == studentReqId);

            if (studentReq == null)
                throw new Exception("Invalid requirement");

            // 🔹 2. Get requirement config
            var req = await _context.UnCourseReqs
                .FirstOrDefaultAsync(x => x.ReqId == studentReq.ReqId);

            if (req == null || string.IsNullOrEmpty(req.RequiredDocuments))
                throw new Exception("No required documents found");

            // 🔹 3. Parse required document IDs
            var requiredDocIds = req.RequiredDocuments
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            // 🔹 4. Get master document details
            var masterDocs = await _context.UnMasterDocs
                .Where(x => requiredDocIds.Contains(x.UniversityDocsId))
                .ToListAsync();

            // 🔹 5. Get uploaded documents
            var uploadedDocs = await _context.StudentDocuments
                .Where(x => x.StudentReqId == studentReqId)
                .ToListAsync();

            // 🔹 6. Map result
            var result = masterDocs.Select(doc =>
            {
                var uploaded = uploadedDocs
                    .FirstOrDefault(x => x.MasterDocId == doc.UniversityDocsId);

                return new StudentDocumentDto
                {
                    StdReqId = studentReqId,
                    MasterDocId = doc.UniversityDocsId,
                    DocName = doc.DocumentName,
                    DocType = doc.DocType ?? "",
                    IsUploaded = uploaded != null,
                    FileUrl = uploaded != null && !string.IsNullOrEmpty(uploaded.FileUrlName)
                    ? _localFileService.GetFileUrl(uploaded.FileUrlName)
                    : null
                };
            }).ToList();

            return result;
        }


        public async Task<List<StudentDocumentDto>> GetDocumentStatusAsync(DocumentStatusRequestDto request)
        {
            // 🔹 1. Validate request
            if (request.ReqId <= 0)
                throw new CustomException("Invalid requirement");

            if (request.StudentReqId == null && request.UploadSessionId == null)
                throw new CustomException("Either StudentReqId or UploadSessionId is required");

            // 🔹 2. Get requirement config
            var req = await _context.UnCourseReqs
                .FirstOrDefaultAsync(x => x.ReqId == request.ReqId);

            if (req == null || string.IsNullOrEmpty(req.RequiredDocuments))
                throw new CustomException("No required documents found");

            // 🔹 3. Parse required docs
            var requiredDocIds = req.RequiredDocuments
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            // 🔹 4. Get master docs
            var masterDocs = await _context.UnMasterDocs
                .Where(x => requiredDocIds.Contains(x.UniversityDocsId))
                .ToListAsync();

            // 🔹 5. Get uploaded docs (CORE LOGIC)
            List<StudentDocument> uploadedDocs;

            if (request.StudentReqId != null)
            {
                // AFTER SAVE
                uploadedDocs = await _context.StudentDocuments
                    .Where(x => x.StudentReqId == request.StudentReqId)
                    .ToListAsync();
            }
            else
            {
                // BEFORE SAVE
                uploadedDocs = await _context.StudentDocuments
                    .Where(x => x.UploadSessionId == request.UploadSessionId && x.StudentReqId == null)
                    .ToListAsync();
            }

            // 🔹 6.Map required documents with uploaded documents
            var result = masterDocs.Select(doc =>
            {
                var uploaded = uploadedDocs
                    .FirstOrDefault(x => x.MasterDocId == doc.UniversityDocsId);

                return new StudentDocumentDto
                {
                    StdReqId = request.StudentReqId,
                    MasterDocId = doc.UniversityDocsId,
                    DocName = doc.DocumentName,
                    DocType = doc.DocType ?? "",
                    IsUploaded = uploaded != null,
                    FileUrl = uploaded != null && !string.IsNullOrEmpty(uploaded.FileUrlName)
                        ? _localFileService.GetFileUrl(uploaded.FileUrlName)
                        : null
                };
            }).ToList();

            return result;
        }




        private async Task<List<long>> ValidateRequiredDocumentsAsync(long studentReqId)
        {
            // 🔹 Get student requirement
            var studentReq = await _context.StudentReqLists
                .FirstOrDefaultAsync(x => x.StudentReqId == studentReqId);

            if (studentReq == null)
                throw new CustomException("Invalid requirement");

            // 🔹 Get requirement config
            var req = await _context.UnCourseReqs
                .FirstOrDefaultAsync(x => x.ReqId == studentReq.ReqId);

            if (req == null || string.IsNullOrEmpty(req.RequiredDocuments))
                throw new CustomException("Requirement documents not configured");

            // 🔹 Parse required docs
            var requiredDocs = req.RequiredDocuments
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            // 🔹 Get uploaded docs
            var uploadedDocs = await _context.StudentDocuments
                .Where(x => x.StudentReqId == studentReqId)
                .Select(x => x.MasterDocId)
                .ToListAsync();

            // 🔹 Find missing docs
            var missingDocs = requiredDocs
                .Where(id => !uploadedDocs.Contains(id))
                .ToList();

            return missingDocs;
        }


        private void ValidateUniversityAndNgoWorkflow(StudentReqList entity, StudentRequirementRequestDto dto)
        {
            // 🔹 Awarding only after documents accepted
            if (dto.UniAwardingStatus == (int)AwardingStatus.Awarded &&
                entity.DocumentStatus != (int)DocumentStatus.Accepted)
            {
                throw new CustomException("Documents must be accepted before awarding");
            }

            // 🔹 Prevent changing document status AFTER awarding
            if (entity.UniAwardingstatus == (int)AwardingStatus.Awarded &&
                dto.DocumentStatus != entity.DocumentStatus)
            {
                throw new CustomException("Document status cannot be changed after awarding");
            }

            // 🔹 Sponsorship only after awarding
            if (dto.DaAdmissionStatus == (int)SponsoredStatus.Sponsored &&
                entity.UniAwardingstatus != (int)AwardingStatus.Awarded)
            {
                throw new CustomException("Student must be awarded before sponsorship");
            }

            // 🔹 Lock everything after sponsorship
            if (entity.DaAdmissionStatus == (int)SponsoredStatus.Sponsored)
            {
                throw new CustomException("Sponsorship already completed. No further updates allowed");
            }
        }



        private string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "doc";

            // remove special chars
            name = Regex.Replace(name, @"[^a-zA-Z0-9\s-]", "");

            // replace spaces with underscore
            name = name.Replace(" ", "_");

            return name.ToLower();
        }

    }
}







//university can update document status(accepted only if all documnst are uploaded) 
//and if document status is accepted then awareding status can be updated by university only 
//    and if awarding status is awarded then sponsorship status can be updated by ngo only.



//    and if sponsored no further update allowed from university or ngo. Because sponsoring already done.
//    and if awarding already done then document status cannot be updated by university. Because awarding already done.



//public async Task<List<StudentDocumentDto>> GetUploadedDocs(long studentReqId)
//{
//    var docs = await _context.StudentDocuments
//        .Include(x => x.MasterDoc)
//        .Where(x => x.StudentReqId == studentReqId)
//        .ToListAsync();

//    return docs.Select(x => new StudentDocumentDto
//    {
//        StdReqId = studentReqId,
//        DocId = x.DocumentId,
//        MasterDocId = x.MasterDocId,
//        DocName = x.MasterDoc.DocumentName,
//        DocType = x.MasterDoc.DocType ?? "",
//        IsUploaded = true, // always true here
//        FileUrl = !string.IsNullOrEmpty(x.FileUrlName)
//            ? _localFileService.GetFileUrl(x.FileUrlName)
//            : null,
//        UploadedAt = x.CreatedDate
//    }).ToList();
//}


