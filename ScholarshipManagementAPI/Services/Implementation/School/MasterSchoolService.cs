using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
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

                IsDraft = dto.IsDraft,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedDate = null,

                // Draft / Submit Logic
                AccreditationStatus = dto.IsDraft ? (byte)0 : (byte)AccreditationStatusEnum.Pending,

                CommitteeComment = null,
                AccreditationBy = null,
                AccreditationDate = null,
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


            // Accreditation / Workflow Logic

            var oldStatus = entity.AccreditationStatus;
            var wasDraft = entity.IsDraft;

            // Do not allow modification while under review
            if (oldStatus == (byte)AccreditationStatusEnum.Pending)
            {
                throw new CustomException("School is under accreditation review.");
            }

            // Do not allow modification after accreditation
            if (oldStatus == (byte)AccreditationStatusEnum.Accredited)
            {
                throw new CustomException("Accredited school cannot be modified.");
            }

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

            // Draft -> Submit
            if (wasDraft && !dto.IsDraft && dto.AccreditationStatus == (byte)AccreditationStatusEnum.Pending)
            {
                entity.IsDraft = false;
                entity.AccreditationStatus = (byte)AccreditationStatusEnum.Pending;
                entity.AccreditationDate = null;
                entity.AccreditationBy = null;
                entity.CommitteeComment = null;
            }

            //entity.IsActive = dto.IsActive;

            entity.UpdatedBy = dto.UpdatedBy;
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

            // Already soft deleted
            if (!entity.IsActive)
            {
                throw new CustomException("School is already deleted.");
            }

            // Do not allow deletion while under accreditation review
            if (entity.AccreditationStatus == (byte)AccreditationStatusEnum.Pending)
            {
                throw new CustomException(
                    "School under accreditation review cannot be deleted."
                );
            }

            // Do not allow deletion after accreditation
            if (entity.AccreditationStatus == (byte)AccreditationStatusEnum.Accredited)
            {
                throw new CustomException(
                    "Accredited school cannot be deleted."
                );
            }

            // Check active child records here, if School has any

            var hasActiveStudents = await _context.KfStudentRegistrations
                .AnyAsync(x => x.SchoolId == id && x.IsActive);

            if (hasActiveStudents)
            {
                throw new CustomException(
                    "This school cannot be deleted because it has active students."
                );
            }


            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<MasterSchoolRequestDto?> GetByIdAsync(long id, LoggedInUserDto currentUser)
        {
            var query = _context.KfSchools
                .AsNoTracking()
                .Where(x => x.SchoolId == id && x.IsActive);

            // Role-based visibility
            if (currentUser.StaffType == StaffType.School)
            {
                // School users can see all active schools
            }
            else if (currentUser.StaffType == StaffType.Ngo)
            {
                // NGO can see all submitted/non-draft schools
                query = query.Where(x => !x.IsDraft);
            }
            else
            {
                // Other users can see only accredited schools
                query = query.Where(x => x.AccreditationStatus == (byte)AccreditationStatusEnum.Accredited);
            }

            return await query
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
                    SchoolType = x.SchoolType,
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
                    SchoolStatus = x.SchoolStatus,
                    SchoolStatusName = x.SchoolStatusNavigation != null ? x.SchoolStatusNavigation.DisplayText : null,
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
        public async Task<PagedResultDto<MasterSchoolRequestDto>> GetByFilterAsync(MasterSchoolFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.KfSchools
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            // Role-based visibility
            if (currentUser.StaffType == StaffType.School)
            {
                // School Coordinator can see all active schools
                // Draft + Pending + Accredited + Rejected

                // Only schools belonging to currentUser.SchoolIds
                query = query.Where(x => currentUser.SchoolIds.Contains(x.SchoolId));
            }
            else if (currentUser.StaffType == StaffType.Ngo)
            {
                // NGO can see all submitted/non-draft schools
                query = query.Where(x => !x.IsDraft);
            }
            else
            {
                // Other users can see only accredited schools
                query = query.Where(x => x.AccreditationStatus == (byte)AccreditationStatusEnum.Accredited);
            }


            // Accreditation filter is meaningful only for
            // School and NGO users.
            if (filter.AccreditationStatus.HasValue &&
                (currentUser.StaffType == StaffType.School ||
                 currentUser.StaffType == StaffType.Ngo))
            {
                query = query.Where(x =>
                    x.AccreditationStatus == filter.AccreditationStatus.Value);
            }

            // Country filter
            if (filter.CountryId.HasValue)
            {
                query = query.Where(x => x.CountryId == filter.CountryId.Value);
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
                    SchoolType = x.SchoolType,
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
                    SchoolStatus = x.SchoolStatus,
                    SchoolStatusName = x.SchoolStatusNavigation != null ? x.SchoolStatusNavigation.DisplayText : null,
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




        // ---------------- GET SCHOOLS BY COUNTRY IDS ----------------
        public async Task<List<SchoolLookupDto>> GetSchoolsByCountryIdsAsync(List<long> countryIds)
        {
            if (countryIds == null || !countryIds.Any())
                return new List<SchoolLookupDto>();

            return await _context.KfSchools
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDraft &&
                    x.AccreditationStatus == (byte)AccreditationStatusEnum.Accredited &&
                    countryIds.Contains(x.CountryId) &&
                    x.Country != null &&
                    x.Country.IsActive)
                .OrderBy(x => x.SchoolName)
                .Select(x => new SchoolLookupDto
                {
                    SchoolId = x.SchoolId,
                    SchoolName = x.SchoolName
                })
                .ToListAsync();
        }



    }
}
