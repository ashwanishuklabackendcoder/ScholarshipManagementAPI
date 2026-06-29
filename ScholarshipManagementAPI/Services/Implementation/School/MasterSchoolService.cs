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

            // Parsing helpers
            byte.TryParse(dto.SchoolType, out var schoolTypeVal);
            short.TryParse(dto.SchoolYearOfEstablish, out var establishYearVal);
            int.TryParse(dto.TotalNumberOfHighSchoolLevel, out var hsTotal);
            int.TryParse(dto.AverageNumberOfStudentPerClass, out var avgStudents);
            int.TryParse(dto.SchoolLocalRank, out var localRank);
            int.TryParse(dto.StudentCodeFormatLastSavedNumber, out var seqNumber);

            bool.TryParse(dto.StudentSuccessAverage, out var successRateAbove80);
            bool.TryParse(dto.AverageSchoolGraduates, out var eligibilityRateAbove80);
            bool.TryParse(dto.GraduatesEnglishLessThan, out var englishAbove80);

            var entity = new KfSchool
            {
                SchoolName = dto.SchoolName,
                ShortName = dto.ShortName,
                SchoolType = schoolTypeVal > 0 ? schoolTypeVal : (byte)1,
                OwningInstitution = dto.OwningInstitution,
                SchoolYearOfEstablish = establishYearVal > 0 ? establishYearVal : null,

                CountryId = dto.CountryId,
                Area = dto.Area,
                CenterName = dto.CenterName,
                SchoolNumber = dto.SchoolNumber,

                AcademicYearStartDate = dto.AcademicYearStartDate,
                AcademicYearEndDate = dto.AcademicYearEndDate,
                SchoolTeachingLanguage = dto.SchoolTeachingLanguage,
                SchoolAccreditations = dto.SchoolAccreditations,
                IsIslamicCurriculum = false, // Set default or check curriculum
                ReligionSubjectCurriculum = dto.SchoolSubjectCurriculum,

                TotalStudentsHighSchool = hsTotal > 0 ? hsTotal : null,
                AverageStudentsPerClass = avgStudents > 0 ? avgStudents : null,
                SchoolLocalRank = localRank > 0 ? localRank : null,
                IsThreeYearStudentSuccessRateAbove80 = successRateAbove80,
                IsUniversityEligibilityRateAbove80 = eligibilityRateAbove80,
                IsGraduateEnglishProficiencyAbove80 = englishAbove80,

                SchoolWebsite = dto.SchoolWebsite,
                SchoolPhoneNo = dto.SchoolPhoneNo,
                EmailId = dto.EmailId,

                PrincipalName = null,
                PrincipalMobile = null,
                PrincipalEmail = null,

                SchoolCoordinatorName = dto.SchoolCoordinatorName,
                SchoolCoordinatorMobile = dto.SchoolCoordinatorMobile,
                SchoolCoordinatorEmail = dto.SchoolCoordinatorEmail,

                DefaultCurrencyId = dto.DefaultCurrencyId,
                SchoolStatus = 1, // Default Active
                StudentCodeFormatPrefix = dto.StudentCodeFormatPrefix,
                StudentCodeFormatSuffix = dto.StudentCodeFormatSufix,
                StudentSequenceNumber = seqNumber > 0 ? seqNumber : 1,

                AccreditationStatus = 1, // Pending/Draft
                AccreditationBy = null,
                AccreditationDate = null,
                CommitteeComment = null,

                IsDraft = true,
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

            // Parsing helpers
            byte.TryParse(dto.SchoolType, out var schoolTypeVal);
            short.TryParse(dto.SchoolYearOfEstablish, out var establishYearVal);
            int.TryParse(dto.TotalNumberOfHighSchoolLevel, out var hsTotal);
            int.TryParse(dto.AverageNumberOfStudentPerClass, out var avgStudents);
            int.TryParse(dto.SchoolLocalRank, out var localRank);
            int.TryParse(dto.StudentCodeFormatLastSavedNumber, out var seqNumber);

            bool.TryParse(dto.StudentSuccessAverage, out var successRateAbove80);
            bool.TryParse(dto.AverageSchoolGraduates, out var eligibilityRateAbove80);
            bool.TryParse(dto.GraduatesEnglishLessThan, out var englishAbove80);

            entity.SchoolName = dto.SchoolName;
            entity.ShortName = dto.ShortName;
            entity.SchoolType = schoolTypeVal > 0 ? schoolTypeVal : (byte)1;
            entity.OwningInstitution = dto.OwningInstitution;
            entity.SchoolYearOfEstablish = establishYearVal > 0 ? establishYearVal : null;

            entity.CountryId = dto.CountryId;
            entity.Area = dto.Area;
            entity.CenterName = dto.CenterName;
            entity.SchoolNumber = dto.SchoolNumber;

            entity.AcademicYearStartDate = dto.AcademicYearStartDate;
            entity.AcademicYearEndDate = dto.AcademicYearEndDate;
            entity.SchoolTeachingLanguage = dto.SchoolTeachingLanguage;
            entity.SchoolAccreditations = dto.SchoolAccreditations;
            entity.ReligionSubjectCurriculum = dto.SchoolSubjectCurriculum;

            entity.TotalStudentsHighSchool = hsTotal > 0 ? hsTotal : null;
            entity.AverageStudentsPerClass = avgStudents > 0 ? avgStudents : null;
            entity.SchoolLocalRank = localRank > 0 ? localRank : null;
            entity.IsThreeYearStudentSuccessRateAbove80 = successRateAbove80;
            entity.IsUniversityEligibilityRateAbove80 = eligibilityRateAbove80;
            entity.IsGraduateEnglishProficiencyAbove80 = englishAbove80;

            entity.SchoolWebsite = dto.SchoolWebsite;
            entity.SchoolPhoneNo = dto.SchoolPhoneNo;
            entity.EmailId = dto.EmailId;

            entity.SchoolCoordinatorName = dto.SchoolCoordinatorName;
            entity.SchoolCoordinatorMobile = dto.SchoolCoordinatorMobile;
            entity.SchoolCoordinatorEmail = dto.SchoolCoordinatorEmail;

            entity.DefaultCurrencyId = dto.DefaultCurrencyId;
            entity.StudentCodeFormatPrefix = dto.StudentCodeFormatPrefix;
            entity.StudentCodeFormatSuffix = dto.StudentCodeFormatSufix;
            entity.StudentSequenceNumber = seqNumber > 0 ? seqNumber : 1;

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
                    StudentCodeFormatSufix = x.StudentCodeFormatSuffix,
                    StudentCodeFormatStartingNo = x.StudentCodeFormatPrefix, // fallback
                    StudentCodeFormatLastSavedNumber = x.StudentSequenceNumber.ToString(),
                    CountryId = x.CountryId,
                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ShortName = x.ShortName,
                    Area = x.Area,
                    CenterName = x.CenterName,
                    SchoolNumber = x.SchoolNumber,
                    SchoolYearOfEstablish = x.SchoolYearOfEstablish.ToString(),
                    SchoolType = x.SchoolType.ToString(),
                    SchoolTeachingLanguage = x.SchoolTeachingLanguage,
                    GraduatesEnglishLessThan = x.IsGraduateEnglishProficiencyAbove80.ToString(),
                    TotalNumberOfHighSchoolLevel = x.TotalStudentsHighSchool.ToString(),
                    AverageNumberOfStudentPerClass = x.AverageStudentsPerClass.ToString(),
                    SchoolAccreditations = x.SchoolAccreditations,
                    SchoolSubjectCurriculum = x.ReligionSubjectCurriculum,
                    StudentSuccessAverage = x.IsThreeYearStudentSuccessRateAbove80.ToString(),
                    AverageSchoolGraduates = x.IsUniversityEligibilityRateAbove80.ToString(),
                    SchoolLocalRank = x.SchoolLocalRank.ToString(),
                    OwningInstitution = x.OwningInstitution,
                    SchoolWebsite = x.SchoolWebsite,
                    SchoolPhoneNo = x.SchoolPhoneNo,
                    EmailId = x.EmailId,
                    IsActive = x.IsActive,
                    SchoolCoordinatorName = x.SchoolCoordinatorName,
                    SchoolCoordinatorMobile = x.SchoolCoordinatorMobile,
                    SchoolCoordinatorEmail = x.SchoolCoordinatorEmail,
                    AcademicYearStartDate = x.AcademicYearStartDate,
                    AcademicYearEndDate = x.AcademicYearEndDate,
                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = x.DefaultCurrencyId,

                    ApprovalStatus = x.AccreditationStatus,
                    ApprovedBy = x.AccreditationBy,
                    ApprovedByName = x.AccreditationByNavigation != null ? x.AccreditationByNavigation.LoginName : null
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
                var from = filter.AcademicYearStartFrom ?? DateTime.MinValue;
                var to = filter.AcademicYearEndTo ?? DateTime.MaxValue;

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
            query = query.OrderByDescending(x => x.StudentData.Count());

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
                    StudentCodeFormatSufix = x.StudentCodeFormatSuffix,
                    StudentCodeFormatStartingNo = x.StudentCodeFormatPrefix,
                    StudentCodeFormatLastSavedNumber = x.StudentSequenceNumber.ToString(),
                    CountryId = x.CountryId,
                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ShortName = x.ShortName,
                    Area = x.Area,
                    CenterName = x.CenterName,
                    SchoolNumber = x.SchoolNumber,
                    SchoolYearOfEstablish = x.SchoolYearOfEstablish.ToString(),
                    SchoolType = x.SchoolType.ToString(),
                    SchoolTeachingLanguage = x.SchoolTeachingLanguage,
                    GraduatesEnglishLessThan = x.IsGraduateEnglishProficiencyAbove80.ToString(),
                    TotalNumberOfHighSchoolLevel = x.TotalStudentsHighSchool.ToString(),
                    AverageNumberOfStudentPerClass = x.AverageStudentsPerClass.ToString(),
                    SchoolAccreditations = x.SchoolAccreditations,
                    SchoolSubjectCurriculum = x.ReligionSubjectCurriculum,
                    StudentSuccessAverage = x.IsThreeYearStudentSuccessRateAbove80.ToString(),
                    AverageSchoolGraduates = x.IsUniversityEligibilityRateAbove80.ToString(),
                    SchoolLocalRank = x.SchoolLocalRank.ToString(),
                    OwningInstitution = x.OwningInstitution,
                    SchoolWebsite = x.SchoolWebsite,
                    SchoolPhoneNo = x.SchoolPhoneNo,
                    EmailId = x.EmailId,
                    IsActive = x.IsActive,
                    SchoolCoordinatorName = x.SchoolCoordinatorName,
                    SchoolCoordinatorMobile = x.SchoolCoordinatorMobile,
                    SchoolCoordinatorEmail = x.SchoolCoordinatorEmail,
                    AcademicYearStartDate = x.AcademicYearStartDate,
                    AcademicYearEndDate = x.AcademicYearEndDate,
                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = x.DefaultCurrencyId,

                    ApprovalStatus = x.AccreditationStatus,
                    ApprovedBy = x.AccreditationBy,
                    ApprovedByName = x.AccreditationByNavigation != null ? x.AccreditationByNavigation.LoginName : null,

                    TotalStudents = x.StudentData != null ? x.StudentData.Count : 0
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
