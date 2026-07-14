using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class MasterSchoolService : IMasterSchoolService
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserContextService _currentUserContext;

        public MasterSchoolService(AppDbContext context, CurrentUserContextService currentUserContext)
        {
            _context = context;
            _currentUserContext = currentUserContext;
        }

        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(MasterSchoolRequestDto dto)
        {
            if (await _context.KfSchools
                .AnyAsync(x => x.SchoolName.ToLower() == dto.SchoolName.ToLower()))
            {
                throw new CustomException("School with same name already exists");
            }

            long currentUserId = 2;
            try
            {
                var user = await _currentUserContext.GetCurrentUserAsync();
                if (user != null) currentUserId = user.LoginId;
            }
            catch { }

            var entity = new KfSchool
            {
                SchoolName = dto.SchoolName,
                ShortName = dto.ShortName,
                SchoolType = dto.SchoolType,
                OwningInstitution = dto.OwningInstitution,
                SchoolYearOfEstablish = dto.SchoolYearOfEstablish,

                CountryId = dto.CountryId,
                Area = dto.Area,
                CenterName = dto.CenterName,
                SchoolNumber = dto.SchoolNumber,

                AcademicYearStartDate = dto.AcademicYearStartDate.HasValue ? DateOnly.FromDateTime(dto.AcademicYearStartDate.Value) : null,
                AcademicYearEndDate = dto.AcademicYearEndDate.HasValue ? DateOnly.FromDateTime(dto.AcademicYearEndDate.Value) : null,
                SchoolTeachingLanguage = dto.SchoolTeachingLanguage,
                SchoolAccreditations = dto.SchoolAccreditations,
                IsIslamicCurriculum = dto.IsIslamicCurriculum,
                ReligionSubjectCurriculum = dto.ReligionSubjectCurriculum,

                TotalStudentsHighSchool = dto.TotalStudentsHighSchool,
                AverageStudentsPerClass = dto.AverageStudentsPerClass,
                SchoolLocalRank = dto.SchoolLocalRank,
                IsThreeYearStudentSuccessRateAbove80 = dto.IsThreeYearStudentSuccessRateAbove80,
                IsUniversityEligibilityRateAbove80 = dto.IsUniversityEligibilityRateAbove80,
                IsGraduateEnglishProficiencyAbove80 = dto.IsGraduateEnglishProficiencyAbove80,

                SchoolWebsite = dto.SchoolWebsite,
                SchoolPhoneNo = dto.SchoolPhoneNo,
                EmailId = dto.EmailId,

                PrincipalName = dto.PrincipalName,
                PrincipalMobile = dto.PrincipalMobile,
                PrincipalEmail = dto.PrincipalEmail,

                SchoolCoordinatorName = dto.SchoolCoordinatorName,
                SchoolCoordinatorMobile = dto.SchoolCoordinatorMobile,
                SchoolCoordinatorEmail = dto.SchoolCoordinatorEmail,

                DefaultCurrencyId = dto.DefaultCurrencyId,
                SchoolStatus = dto.SchoolStatus,
                StudentCodeFormatPrefix = dto.StudentCodeFormatPrefix,
                StudentCodeFormatSuffix = dto.StudentCodeFormatSuffix,
                StudentSequenceNumber = dto.StudentSequenceNumber,

                AccreditationStatus = dto.AccreditationStatus,
                AccreditationBy = dto.AccreditationBy,
                AccreditationDate = dto.AccreditationDate,
                CommitteeComment = dto.CommitteeComment,

                IsDraft = dto.IsDraft,
                IsActive = dto.IsActive,
                CreatedBy = currentUserId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedDate = null
            };

            _context.KfSchools.Add(entity);
            await _context.SaveChangesAsync();

            return entity.SchoolId;
        }

        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(MasterSchoolRequestDto dto)
        {
            if (dto.SchoolId == null || dto.SchoolId == 0)
                return false;

            if (await _context.KfSchools.AnyAsync(x =>
                      x.SchoolName.ToLower() == dto.SchoolName.ToLower()
                      && x.SchoolId != dto.SchoolId))
            {
                throw new CustomException("School with same name already exists");
            }

            var entity = await _context.KfSchools
                .FirstOrDefaultAsync(x => x.SchoolId == dto.SchoolId);

            if (entity == null)
                return false;

            long currentUserId = 2;
            try
            {
                var user = await _currentUserContext.GetCurrentUserAsync();
                if (user != null) currentUserId = user.LoginId;
            }
            catch { }

            entity.SchoolName = dto.SchoolName;
            entity.ShortName = dto.ShortName;
            entity.SchoolType = dto.SchoolType;
            entity.OwningInstitution = dto.OwningInstitution;
            entity.SchoolYearOfEstablish = dto.SchoolYearOfEstablish;

            entity.CountryId = dto.CountryId;
            entity.Area = dto.Area;
            entity.CenterName = dto.CenterName;
            entity.SchoolNumber = dto.SchoolNumber;

            entity.AcademicYearStartDate = dto.AcademicYearStartDate.HasValue ? DateOnly.FromDateTime(dto.AcademicYearStartDate.Value) : null;
            entity.AcademicYearEndDate = dto.AcademicYearEndDate.HasValue ? DateOnly.FromDateTime(dto.AcademicYearEndDate.Value) : null;
            entity.SchoolTeachingLanguage = dto.SchoolTeachingLanguage;
            entity.SchoolAccreditations = dto.SchoolAccreditations;
            entity.IsIslamicCurriculum = dto.IsIslamicCurriculum;
            entity.ReligionSubjectCurriculum = dto.ReligionSubjectCurriculum;

            entity.TotalStudentsHighSchool = dto.TotalStudentsHighSchool;
            entity.AverageStudentsPerClass = dto.AverageStudentsPerClass;
            entity.SchoolLocalRank = dto.SchoolLocalRank;
            entity.IsThreeYearStudentSuccessRateAbove80 = dto.IsThreeYearStudentSuccessRateAbove80;
            entity.IsUniversityEligibilityRateAbove80 = dto.IsUniversityEligibilityRateAbove80;
            entity.IsGraduateEnglishProficiencyAbove80 = dto.IsGraduateEnglishProficiencyAbove80;

            entity.SchoolWebsite = dto.SchoolWebsite;
            entity.SchoolPhoneNo = dto.SchoolPhoneNo;
            entity.EmailId = dto.EmailId;

            entity.PrincipalName = dto.PrincipalName;
            entity.PrincipalMobile = dto.PrincipalMobile;
            entity.PrincipalEmail = dto.PrincipalEmail;

            entity.SchoolCoordinatorName = dto.SchoolCoordinatorName;
            entity.SchoolCoordinatorMobile = dto.SchoolCoordinatorMobile;
            entity.SchoolCoordinatorEmail = dto.SchoolCoordinatorEmail;

            entity.DefaultCurrencyId = dto.DefaultCurrencyId;
            entity.SchoolStatus = dto.SchoolStatus;
            entity.StudentCodeFormatPrefix = dto.StudentCodeFormatPrefix;
            entity.StudentCodeFormatSuffix = dto.StudentCodeFormatSuffix;
            entity.StudentSequenceNumber = dto.StudentSequenceNumber;

            entity.AccreditationStatus = dto.AccreditationStatus;
            entity.AccreditationBy = dto.AccreditationBy;
            entity.AccreditationDate = dto.AccreditationDate;
            entity.CommitteeComment = dto.CommitteeComment;

            entity.IsDraft = dto.IsDraft;
            entity.IsActive = dto.IsActive;

            entity.UpdatedBy = currentUserId;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.KfSchools
                .FirstOrDefaultAsync(x => x.SchoolId == id);

            if (entity == null)
                return false;

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }

        // ---------------- GET BY ID ----------------
        public async Task<MasterSchoolRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfSchools
                .AsNoTracking()
                .Where(x => x.SchoolId == id)
                .Select(x => new MasterSchoolRequestDto
                {
                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolName,
                    StudentCodeFormatPrefix = x.StudentCodeFormatPrefix,
                    StudentCodeFormatSuffix = x.StudentCodeFormatSuffix,
                    StudentSequenceNumber = x.StudentSequenceNumber,
                    CountryId = x.CountryId,
                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ShortName = x.ShortName,
                    Area = x.Area,
                    CenterName = x.CenterName,
                    SchoolNumber = x.SchoolNumber,
                    SchoolYearOfEstablish = x.SchoolYearOfEstablish,
                    SchoolType = (byte)x.SchoolType,
                    SchoolTeachingLanguage = x.SchoolTeachingLanguage,
                    IsIslamicCurriculum = x.IsIslamicCurriculum,
                    ReligionSubjectCurriculum = x.ReligionSubjectCurriculum,
                    TotalStudentsHighSchool = x.TotalStudentsHighSchool,
                    AverageStudentsPerClass = x.AverageStudentsPerClass,
                    SchoolLocalRank = x.SchoolLocalRank,
                    IsThreeYearStudentSuccessRateAbove80 = x.IsThreeYearStudentSuccessRateAbove80,
                    IsUniversityEligibilityRateAbove80 = x.IsUniversityEligibilityRateAbove80,
                    IsGraduateEnglishProficiencyAbove80 = x.IsGraduateEnglishProficiencyAbove80,
                    OwningInstitution = x.OwningInstitution,
                    SchoolWebsite = x.SchoolWebsite,
                    SchoolPhoneNo = x.SchoolPhoneNo,
                    EmailId = x.EmailId,
                    PrincipalName = x.PrincipalName,
                    PrincipalMobile = x.PrincipalMobile,
                    PrincipalEmail = x.PrincipalEmail,
                    IsActive = x.IsActive,
                    IsDraft = x.IsDraft,
                    SchoolCoordinatorName = x.SchoolCoordinatorName,
                    SchoolCoordinatorMobile = x.SchoolCoordinatorMobile,
                    SchoolCoordinatorEmail = x.SchoolCoordinatorEmail,
                    AcademicYearStartDate = x.AcademicYearStartDate.HasValue ? x.AcademicYearStartDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    AcademicYearEndDate = x.AcademicYearEndDate.HasValue ? x.AcademicYearEndDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = x.DefaultCurrencyId,
                    DefaultCurrencyName = x.DefaultCurrency != null ? x.DefaultCurrency.CurrencyName : null,

                    AccreditationStatus = x.AccreditationStatus,
                    AccreditationBy = x.AccreditationBy,
                    AccreditationByName = x.AccreditationByNavigation != null ? x.AccreditationByNavigation.LoginName : null
                })
                .FirstOrDefaultAsync();
        }

        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<MasterSchoolRequestDto>> GetByFilterAsync(MasterSchoolFilterDto filter)
        {
            var query = _context.KfSchools
                .AsNoTracking()
                .AsQueryable();

            // Country filter
            if (filter.CountryId.HasValue)
            {
                query = query.Where(x => x.CountryId == filter.CountryId.Value);
            }

            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            // Approval status filter
            if (filter.ApprovalStatus.HasValue)
            {
                query = query.Where(x => x.AccreditationStatus == filter.ApprovalStatus.Value);
            }

            // filter date range filter
            if (filter.AcademicYearStartFrom.HasValue || filter.AcademicYearEndTo.HasValue)
            {
                var from = DateOnly.FromDateTime(filter.AcademicYearStartFrom ?? DateTime.MinValue);
                var to = DateOnly.FromDateTime(filter.AcademicYearEndTo ?? DateTime.MaxValue);

                query = query.Where(x =>
                    x.AcademicYearStartDate.HasValue &&
                    x.AcademicYearEndDate.HasValue &&
                    x.AcademicYearEndDate.Value >= from &&
                    x.AcademicYearStartDate.Value <= to
                );
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.SchoolName.ToLower().Contains(search) ||
                    (x.ShortName != null && x.ShortName.ToLower().Contains(search)) ||
                    (x.Area != null && x.Area.ToLower().Contains(search)) ||
                    (x.CenterName != null && x.CenterName.ToLower().Contains(search)) ||
                    (x.SchoolWebsite != null && x.SchoolWebsite.ToLower().Contains(search)) ||
                    (x.EmailId != null && x.EmailId.ToLower().Contains(search))
                );
            }

            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.SchoolId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new MasterSchoolRequestDto
                {
                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolName,
                    StudentCodeFormatPrefix = x.StudentCodeFormatPrefix,
                    StudentCodeFormatSuffix = x.StudentCodeFormatSuffix,
                    StudentSequenceNumber = x.StudentSequenceNumber,
                    CountryId = x.CountryId,
                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ShortName = x.ShortName,
                    Area = x.Area,
                    CenterName = x.CenterName,
                    SchoolNumber = x.SchoolNumber,
                    SchoolYearOfEstablish = x.SchoolYearOfEstablish,
                    SchoolType = (byte)x.SchoolType,
                    SchoolTeachingLanguage = x.SchoolTeachingLanguage,
                    IsIslamicCurriculum = x.IsIslamicCurriculum,
                    ReligionSubjectCurriculum = x.ReligionSubjectCurriculum,
                    TotalStudentsHighSchool = x.TotalStudentsHighSchool,
                    AverageStudentsPerClass = x.AverageStudentsPerClass,
                    SchoolLocalRank = x.SchoolLocalRank,
                    IsThreeYearStudentSuccessRateAbove80 = x.IsThreeYearStudentSuccessRateAbove80,
                    IsUniversityEligibilityRateAbove80 = x.IsUniversityEligibilityRateAbove80,
                    IsGraduateEnglishProficiencyAbove80 = x.IsGraduateEnglishProficiencyAbove80,
                    OwningInstitution = x.OwningInstitution,
                    SchoolWebsite = x.SchoolWebsite,
                    SchoolPhoneNo = x.SchoolPhoneNo,
                    EmailId = x.EmailId,
                    PrincipalName = x.PrincipalName,
                    PrincipalMobile = x.PrincipalMobile,
                    PrincipalEmail = x.PrincipalEmail,
                    IsActive = x.IsActive,
                    IsDraft = x.IsDraft,
                    SchoolCoordinatorName = x.SchoolCoordinatorName,
                    SchoolCoordinatorMobile = x.SchoolCoordinatorMobile,
                    SchoolCoordinatorEmail = x.SchoolCoordinatorEmail,
                    AcademicYearStartDate = x.AcademicYearStartDate.HasValue ? x.AcademicYearStartDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    AcademicYearEndDate = x.AcademicYearEndDate.HasValue ? x.AcademicYearEndDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = x.DefaultCurrencyId,
                    DefaultCurrencyName = x.DefaultCurrency != null ? x.DefaultCurrency.CurrencyName : null,

                    AccreditationStatus = x.AccreditationStatus,
                    AccreditationBy = x.AccreditationBy,
                    AccreditationByName = x.AccreditationByNavigation != null ? x.AccreditationByNavigation.LoginName : null,

                   // TotalStudents = x.StudentData != null ? x.StudentData.Count : 0
                })
                .ToListAsync();

            return new PagedResultDto<MasterSchoolRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
    }
}
