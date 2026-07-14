using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(StudentRequestDto dto)
        {
            if (await _context.StudentRegistrations.AnyAsync(x => x.Phone == dto.MobileNo))
            {
                throw new CustomException("Student with same mobile number already exists");
            }

            if (await _context.StudentRegistrations.AnyAsync(x => x.Email == dto.EmailID))
            {
                throw new CustomException("Student with same email already exists");
            }

            // Find school name from school id if possible
            string? schoolName = null;
            if (dto.SchoolId > 0)
            {
                schoolName = await _context.KfSchools
                    .Where(x => x.SchoolId == dto.SchoolId)
                    .Select(x => x.SchoolName)
                    .FirstOrDefaultAsync();
            }

            var entity = new StudentRegistration
            {
                FirstName = dto.StudentFirstName,
                LastName = dto.StudentLastName ?? string.Empty,
                MotherName = dto.MotherName,
                Dob = dto.DateOfBirth.HasValue ? DateOnly.FromDateTime(dto.DateOfBirth.Value) : null,
                Gender = dto.Gender.ToString(),
                Tribe = dto.Tribe,
                Nationality = string.IsNullOrWhiteSpace(dto.Nationality) ? null : (long.TryParse(dto.Nationality, out var nat) ? nat : null),
                ResidenceCountry = string.IsNullOrWhiteSpace(dto.MasterCountry) ? null : (long.TryParse(dto.MasterCountry, out var rc) ? rc : null),
                City = dto.AddressCity,
                Phone = dto.MobileNo,
                Email = dto.EmailID,
                PhotoPath = dto.Photo,
                IsOrphan = dto.IsOrphan,
                OrphanNumber = dto.OrphanNumber,
                Religion = dto.Religion.ToString(),
                SchoolName = schoolName,
                HsSpecialization = dto.HighSchoolDiv,
                CombinedSpec = dto.TanzComb,
                TotalScore = string.IsNullOrWhiteSpace(dto.GraduationScore) ? null : (decimal.TryParse(dto.GraduationScore, out var ts) ? ts : null),
                MaxScore = dto.MaxMarks,
                RelativeGrade = string.IsNullOrWhiteSpace(dto.Grade) ? null : (decimal.TryParse(dto.Grade, out var rg) ? rg : null),
                EnglishScore = dto.EnglishPlacementTest,
                FinancialNeed = dto.SocialEcoStatus,
                SelfReliance = dto.SelfDettoSuccess,
                Motivation = dto.MotLevelToOverComedStudying,
                FutureGoals = dto.ClearTargetsFutureGoals,
                RecommendationLetterPath = dto.RecommendationLetterPath,
                RecommendationLetterNotes = dto.RecommendationLetter,
                IsDraft = false,
                IsActive = true,
                CreatedBy = 2,
                CreatedDate = DateTime.UtcNow
            };

            _context.StudentRegistrations.Add(entity);
            await _context.SaveChangesAsync();

            return entity.StudentId;
        }

        public async Task<bool> UpdateAsync(StudentRequestDto dto)
        {
            if (dto.StudentId == null || dto.StudentId == 0)
                return false;

            var entity = await _context.StudentRegistrations
                .FirstOrDefaultAsync(x => x.StudentId == dto.StudentId);

            if (entity == null)
                return false;

            if (await _context.StudentRegistrations.AnyAsync(x => x.Phone == dto.MobileNo && x.StudentId != dto.StudentId))
            {
                throw new CustomException("Student with same mobile number already exists");
            }

            if (await _context.StudentRegistrations.AnyAsync(x => x.Email == dto.EmailID && x.StudentId != dto.StudentId))
            {
                throw new CustomException("Student with same email already exists");
            }

            string? schoolName = null;
            if (dto.SchoolId > 0)
            {
                schoolName = await _context.KfSchools
                    .Where(x => x.SchoolId == dto.SchoolId)
                    .Select(x => x.SchoolName)
                    .FirstOrDefaultAsync();
            }

            entity.FirstName = dto.StudentFirstName;
            entity.LastName = dto.StudentLastName ?? string.Empty;
            entity.MotherName = dto.MotherName;
            entity.Dob = dto.DateOfBirth.HasValue ? DateOnly.FromDateTime(dto.DateOfBirth.Value) : null;
            entity.Gender = dto.Gender.ToString();
            entity.Tribe = dto.Tribe;
            entity.Nationality = string.IsNullOrWhiteSpace(dto.Nationality) ? null : (long.TryParse(dto.Nationality, out var nat) ? nat : null);
            entity.ResidenceCountry = string.IsNullOrWhiteSpace(dto.MasterCountry) ? null : (long.TryParse(dto.MasterCountry, out var rc) ? rc : null);
            entity.City = dto.AddressCity;
            entity.Phone = dto.MobileNo;
            entity.Email = dto.EmailID;
            entity.PhotoPath = dto.Photo;
            entity.IsOrphan = dto.IsOrphan;
            entity.OrphanNumber = dto.OrphanNumber;
            entity.Religion = dto.Religion.ToString();
            entity.SchoolName = schoolName;
            entity.HsSpecialization = dto.HighSchoolDiv;
            entity.CombinedSpec = dto.TanzComb;
            entity.TotalScore = string.IsNullOrWhiteSpace(dto.GraduationScore) ? null : (decimal.TryParse(dto.GraduationScore, out var ts) ? ts : null);
            entity.MaxScore = dto.MaxMarks;
            entity.RelativeGrade = string.IsNullOrWhiteSpace(dto.Grade) ? null : (decimal.TryParse(dto.Grade, out var rg) ? rg : null);
            entity.EnglishScore = dto.EnglishPlacementTest;
            entity.FinancialNeed = dto.SocialEcoStatus;
            entity.SelfReliance = dto.SelfDettoSuccess;
            entity.Motivation = dto.MotLevelToOverComedStudying;
            entity.FutureGoals = dto.ClearTargetsFutureGoals;
            entity.RecommendationLetterPath = dto.RecommendationLetterPath;
            entity.RecommendationLetterNotes = dto.RecommendationLetter;

            entity.UpdatedBy = 2;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.StudentRegistrations
                .FirstOrDefaultAsync(x => x.StudentId == id);

            if (entity == null)
                return false;

            entity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentRequestDto?> GetByIdAsync(long id)
        {
            var x = await _context.StudentRegistrations
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.StudentId == id && u.IsActive);

            if (x == null) return null;

            int? genderInt = null;
            if (int.TryParse(x.Gender, out var g)) genderInt = g;

            long? religionLong = null;
            if (long.TryParse(x.Religion, out var r)) religionLong = r;

            return new StudentRequestDto
            {
                StudentId = x.StudentId,
                StudentFirstName = x.FirstName,
                StudentLastName = x.LastName,
                MotherName = x.MotherName,
                DateOfBirth = x.Dob.HasValue ? x.Dob.Value.ToDateTime(TimeOnly.MinValue) : null,
                Gender = genderInt,
                Tribe = x.Tribe,
                Nationality = x.Nationality?.ToString(),
                MasterCountry = x.ResidenceCountry?.ToString(),
                AddressCity = x.City,
                MobileNo = x.Phone,
                EmailID = x.Email,
                Photo = x.PhotoPath,
                IsOrphan = x.IsOrphan,
                OrphanNumber = x.OrphanNumber,
                Religion = religionLong,
                HighSchoolDiv = x.HsSpecialization,
                TanzComb = x.CombinedSpec,
                GraduationScore = x.TotalScore?.ToString(),
                MaxMarks = x.MaxScore,
                Grade = x.RelativeGrade?.ToString(),
                EnglishPlacementTest = x.EnglishScore,
                SocialEcoStatus = x.FinancialNeed,
                SelfDettoSuccess = x.SelfReliance,
                MotLevelToOverComedStudying = x.Motivation,
                ClearTargetsFutureGoals = x.FutureGoals,
                RecommendationLetterPath = x.RecommendationLetterPath,
                RecommendationLetter = x.RecommendationLetterNotes,
                CreatedDate = x.CreatedDate
            };
        }

        public async Task<PagedResultDto<StudentRequestDto>> GetByFilterAsync(StudentFilterDto filter)
        {
            var query = _context.StudentRegistrations
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(search) ||
                    x.LastName.ToLower().Contains(search) ||
                    (x.Email != null && x.Email.ToLower().Contains(search))
                );
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(x => x.StudentId);

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
                    StudentFirstName = x.FirstName,
                    StudentLastName = x.LastName,
                    MotherName = x.MotherName,
                    DateOfBirth = x.Dob.HasValue ? x.Dob.Value.ToDateTime(TimeOnly.MinValue) : null,
                    Tribe = x.Tribe,
                    Nationality = x.Nationality.HasValue ? x.Nationality.Value.ToString() : null,
                    MasterCountry = x.ResidenceCountry.HasValue ? x.ResidenceCountry.Value.ToString() : null,
                    AddressCity = x.City,
                    MobileNo = x.Phone,
                    EmailID = x.Email,
                    Photo = x.PhotoPath,
                    IsOrphan = x.IsOrphan,
                    OrphanNumber = x.OrphanNumber,
                    HighSchoolDiv = x.HsSpecialization,
                    TanzComb = x.CombinedSpec,
                    GraduationScore = x.TotalScore.ToString(),
                    MaxMarks = x.MaxScore,
                    Grade = x.RelativeGrade.ToString(),
                    EnglishPlacementTest = x.EnglishScore,
                    SocialEcoStatus = x.FinancialNeed,
                    SelfDettoSuccess = x.SelfReliance,
                    MotLevelToOverComedStudying = x.Motivation,
                    ClearTargetsFutureGoals = x.FutureGoals,
                    RecommendationLetterPath = x.RecommendationLetterPath,
                    RecommendationLetter = x.RecommendationLetterNotes,
                    CreatedDate = x.CreatedDate
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
    }
}
