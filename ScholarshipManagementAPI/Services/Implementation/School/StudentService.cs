using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public StudentService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(StudentRequestDto dto)
        {
            if (await _context.StudentData
                .AnyAsync(x => x.MobileNo == dto.MobileNo))
            {
                throw new CustomException("Student with same mobile number already exists");
            }

            if (await _context.StudentData
                .AnyAsync(x => x.EmailId == dto.EmailID))
            {
                throw new CustomException("Student with same email already exists");
            }

            var studentNumber = await GenerateStudentNumberAsync(dto.SchoolId);

            var filePath = await UploadRecommendationLetterAsync(
                dto.RecommendationLetterFile,
                studentNumber
            );

            var entity = new StudentDatum
            {
                SchoolId = dto.SchoolId,
                StudentNumber = studentNumber,
                StudentSalutation = dto.StudentSalutation,
                StudentFirstName = dto.StudentFirstName,
                StudentLastName = dto.StudentLastName,
                StudentOtherName = dto.StudentOtherName,
                Nin = dto.NIN,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Tribe = dto.Tribe,
                Nationality = dto.Nationality,
                Address = dto.Address,
                AddressCity = dto.AddressCity,
                MasterState = dto.MasterState,
                MasterCountry = dto.MasterCountry,
                ZipCode = dto.ZipCode,
                MobileNo = dto.MobileNo,
                EmailId = dto.EmailID,
                Photo = dto.Photo,
                IsOrphan = dto.IsOrphan,
                OrphanNumber = dto.OrphanNumber,
                Religion = dto.Religion,
                GraduationScore = dto.GraduationScore,
                Grade = dto.Grade,
                HighSchoolDiv = dto.HighSchoolDiv,
                TanzComb = dto.TanzComb,
                FatherName = dto.FatherName,
                MotherName = dto.MotherName,
                GuardianName = dto.GuardianName,
                SocialEcoStatus = dto.SocialEcoStatus,
                RecommendationLetter = dto.RecommendationLetter,
                SelfDettoSuccess = dto.SelfDettoSuccess,
                MotLevelToOverComedStudying = dto.MotLevelToOverComedStudying,
                ClearTargetsFutureGoals = dto.ClearTargetsFutureGoals,
                MaxMarks = dto.MaxMarks,
                EnglishPlacementTest = dto.EnglishPlacementTest,
                RecommendationLetterPath = filePath,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate
            };

            _context.StudentData.Add(entity);
            await _context.SaveChangesAsync();

            return entity.StudentId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(StudentRequestDto dto)
        {
            if (dto.StudentId == null || dto.StudentId == 0)
                return false;

            var entity = await _context.StudentData
                .FirstOrDefaultAsync(x => x.StudentId == dto.StudentId);

            if (entity == null)
                return false;


            if (await _context.StudentData
                .AnyAsync(x => x.MobileNo == dto.MobileNo && x.StudentId != dto.StudentId))
            {
                throw new CustomException("Student with same mobile number already exists");
            }

            if (await _context.StudentData
                .AnyAsync(x => x.EmailId == dto.EmailID && x.StudentId != dto.StudentId))
            {
                throw new CustomException("Student with same email already exists");
            }


            var filePath = await UploadRecommendationLetterAsync(
                dto.RecommendationLetterFile,
                entity.StudentNumber
            );


            // entity.StudentId = dto.StudentId.Value;
            // entity.SchoolId = dto.SchoolId.Value;
            // entity.StudentNumber = dto.StudentNumber;

            entity.StudentSalutation = dto.StudentSalutation;
            entity.StudentFirstName = dto.StudentFirstName;
            entity.StudentLastName = dto.StudentLastName;
            entity.StudentOtherName = dto.StudentOtherName;
            entity.Nin = dto.NIN;
            entity.DateOfBirth = dto.DateOfBirth;
            entity.Gender = dto.Gender;
            entity.Tribe = dto.Tribe;
            entity.Nationality = dto.Nationality;
            entity.Address = dto.Address;
            entity.AddressCity = dto.AddressCity;
            entity.MasterState = dto.MasterState;
            entity.MasterCountry = dto.MasterCountry;
            entity.ZipCode = dto.ZipCode;
            entity.MobileNo = dto.MobileNo;
            entity.EmailId = dto.EmailID;
            entity.Photo = dto.Photo;
            entity.IsOrphan = dto.IsOrphan;
            entity.OrphanNumber = dto.OrphanNumber;
            entity.Religion = dto.Religion;
            entity.GraduationScore = dto.GraduationScore;
            entity.Grade = dto.Grade;
            entity.HighSchoolDiv = dto.HighSchoolDiv;
            entity.TanzComb = dto.TanzComb;
            entity.FatherName = dto.FatherName;
            entity.MotherName = dto.MotherName;
            entity.GuardianName = dto.GuardianName;
            entity.SocialEcoStatus = dto.SocialEcoStatus;
            entity.RecommendationLetter = dto.RecommendationLetter;
            entity.SelfDettoSuccess = dto.SelfDettoSuccess;
            entity.MotLevelToOverComedStudying = dto.MotLevelToOverComedStudying;
            entity.ClearTargetsFutureGoals = dto.ClearTargetsFutureGoals;
            entity.MaxMarks = dto.MaxMarks;
            entity.EnglishPlacementTest = dto.EnglishPlacementTest;
            entity.RecommendationLetterPath = filePath;

            // entity.CreatedDate = dto.CreatedDate;         // usually not updated
            // entity.CreatedBy = dto.CreatedBy;             // usually not updated

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.StudentData
                .FirstOrDefaultAsync(x => x.StudentId == id);

            if (entity == null)
                return false;

            _context.StudentData.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<StudentRequestDto?> GetByIdAsync(long id)
        {
            var baseUrl = _configuration["AppSettings:BackendUrl"]?.TrimEnd('/');

            return await _context.StudentData
                .Include(x => x.School)
                .AsNoTracking()
                .Where(x => x.StudentId == id)
                .Select(x => new StudentRequestDto
                {
                    StudentId = x.StudentId,
                    SchoolId = x.SchoolId,
                    StudentNumber = x.StudentNumber,
                    StudentSalutation = x.StudentSalutation,
                    StudentFirstName = x.StudentFirstName,
                    StudentLastName = x.StudentLastName,
                    StudentOtherName = x.StudentOtherName,
                    NIN = x.Nin,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender,
                    Tribe = x.Tribe,
                    Nationality = x.Nationality,
                    Address = x.Address,
                    AddressCity = x.AddressCity,
                    MasterState = x.MasterState,
                    MasterCountry = x.MasterCountry,
                    ZipCode = x.ZipCode,
                    MobileNo = x.MobileNo,
                    EmailID = x.EmailId,
                    Photo = x.Photo,
                    IsOrphan = x.IsOrphan,
                    OrphanNumber = x.OrphanNumber,
                    Religion = x.Religion,
                    GraduationScore = x.GraduationScore,
                    Grade = x.Grade,
                    HighSchoolDiv = x.HighSchoolDiv,
                    TanzComb = x.TanzComb,
                    FatherName = x.FatherName,
                    MotherName = x.MotherName,
                    GuardianName = x.GuardianName,
                    SocialEcoStatus = x.SocialEcoStatus,
                    RecommendationLetter = x.RecommendationLetter,
                    SelfDettoSuccess = x.SelfDettoSuccess,
                    MotLevelToOverComedStudying = x.MotLevelToOverComedStudying,
                    ClearTargetsFutureGoals = x.ClearTargetsFutureGoals,
                    MaxMarks = x.MaxMarks,
                    EnglishPlacementTest = x.EnglishPlacementTest,
                    RecommendationLetterPath = x.RecommendationLetterPath != null ? $"{baseUrl}/{x.RecommendationLetterPath}" : null,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    SchoolName = x.School != null ? x.School.SchoolName : null,
                    ShortName = x.School != null ? x.School.ShortName : null,
                })
                .FirstOrDefaultAsync();
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<StudentRequestDto>> GetByFilterAsync(StudentFilterDto filter)
        {
            var baseUrl = _configuration["AppSettings:BackendUrl"]?.TrimEnd('/');

            var query = _context.StudentData
                .Include(x => x.School)
                .AsNoTracking()
                .AsQueryable();

            // Country filter
            if (filter.SchoolId.HasValue)
            {
                query = query.Where(x => x.SchoolId == filter.SchoolId.Value);
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.StudentNumber.ToLower().Contains(search) ||
                    x.StudentFirstName.ToLower().Contains(search) ||
                    (x.StudentLastName != null && x.StudentLastName.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.StudentId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new StudentRequestDto
                {
                    StudentId = x.StudentId,
                    SchoolId = x.SchoolId,
                    StudentNumber = x.StudentNumber,
                    StudentSalutation = x.StudentSalutation,
                    StudentFirstName = x.StudentFirstName,
                    StudentLastName = x.StudentLastName,
                    StudentOtherName = x.StudentOtherName,
                    NIN = x.Nin,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender,
                    Tribe = x.Tribe,
                    Nationality = x.Nationality,
                    Address = x.Address,
                    AddressCity = x.AddressCity,
                    MasterState = x.MasterState,
                    MasterCountry = x.MasterCountry,
                    ZipCode = x.ZipCode,
                    MobileNo = x.MobileNo,
                    EmailID = x.EmailId,
                    Photo = x.Photo,
                    IsOrphan = x.IsOrphan,
                    OrphanNumber = x.OrphanNumber,
                    Religion = x.Religion,
                    GraduationScore = x.GraduationScore,
                    Grade = x.Grade,
                    HighSchoolDiv = x.HighSchoolDiv,
                    TanzComb = x.TanzComb,
                    FatherName = x.FatherName,
                    MotherName = x.MotherName,
                    GuardianName = x.GuardianName,
                    SocialEcoStatus = x.SocialEcoStatus,
                    RecommendationLetter = x.RecommendationLetter,
                    SelfDettoSuccess = x.SelfDettoSuccess,
                    MotLevelToOverComedStudying = x.MotLevelToOverComedStudying,
                    ClearTargetsFutureGoals = x.ClearTargetsFutureGoals,
                    MaxMarks = x.MaxMarks,
                    EnglishPlacementTest = x.EnglishPlacementTest,
                    RecommendationLetterPath = x.RecommendationLetterPath != null ? $"{baseUrl}/{x.RecommendationLetterPath}" : null,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    SchoolName = x.School != null ? x.School.SchoolName : null,
                    ShortName = x.School != null ? x.School.ShortName : null,

                })
                .ToListAsync();

            return new PagedResultDto<StudentRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




        #region Private Methods

        private async Task<string> GenerateStudentNumberAsync(long schoolId)
        {
            // Start transaction (important for concurrency)
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Lock row to prevent duplicate sequence
            var school = await _context.KfSchools
                .FromSqlRaw("SELECT * FROM kf_schools WITH (UPDLOCK, ROWLOCK) WHERE SchoolID = {0}", schoolId)
                .FirstOrDefaultAsync();

            if (school == null)
                throw new CustomException("School not found");

            if (string.IsNullOrWhiteSpace(school.ShortName))
                throw new CustomException("School short name is missing");

            // Increment sequence
            school.StudentSequenceNumber += 1;

            // Generate Student Number
            var year = DateTime.UtcNow.Year;
            var studentNumber = $"{school.ShortName}-{year}-{school.StudentSequenceNumber:D4}";

            // Save changes
            await _context.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();

            return studentNumber;
        }



        private async Task<string?> UploadRecommendationLetterAsync(IFormFile? file, string studentNumber)
        {
            if (file == null)
                return null;

            if (file.Length == 0)
                throw new CustomException("File is empty.");

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                throw new CustomException("Invalid file type. Only PDF/DOC/DOCX allowed.");

            if (file.Length > 5 * 1024 * 1024)
                throw new CustomException("File size must be less than 5MB.");

            var originalName = Path.GetFileName(file.FileName).Replace(" ", "_");
            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}_{originalName}";

            var folder = Path.Combine("wwwroot", "uploads", "students", studentNumber, "recommendations");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/students/{studentNumber}/recommendations/{fileName}";
        }




        #endregion



    }
}
