using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.DTOs.University;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class UniversityService : IUniversityService
    {
        private readonly AppDbContext _context;

        public UniversityService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UniversityRequestDto dto)
        {
            if (await _context.UnUniversityRegistrations
                .AnyAsync(x => x.UniversityName.ToLower() == dto.UniversityName.ToLower()))
            {
                throw new CustomException("University with same name already exists");
            }

            var entity = new UnUniversityRegistration
            {
                UniversityName = dto.UniversityName,
                UniversityType = dto.UniversityTypeId,

                CharterAccreditation = dto.CharterAccreditation,
                EstablishedYear = dto.EstablishedYear,

                CountryId = dto.CountryId,
                City = dto.City,
                Address = dto.Address,
                Website = dto.Website,

                VcName = dto.VcName,
                VcEmail = dto.VcEmail,
                VcMobile = dto.VcMobile,

                CoordName = dto.CoordName,
                CoordPosition = dto.CoordPosition,
                CoordEmail = dto.CoordEmail,
                CoordPhone = dto.CoordPhone,

                FacultiesCount = dto.FacultiesCount,
                FacultyFulltimeCount = dto.FacultyFulltimeCount,
                AdminStaffCount = dto.AdminStaffCount,

                ProgDegreeCount = dto.ProgDegreeCount,
                ProgDiplomaCount = dto.ProgDiplomaCount,
                ProgCertificateCount = dto.ProgCertificateCount,
                ProgPostgradCount = dto.ProgPostgradCount,

                StudentsTotal = dto.StudentsTotal,
                StudentsEnrolled = dto.StudentsEnrolled,
                IntlStudentsPct = dto.IntlStudentsPct,
                StudentsGenderTypeId = dto.StudentsGenderTypeId,

                StudDegreeCount = dto.StudDegreeCount,
                StudDiplomaCount = dto.StudDiplomaCount,
                StudCertificateCount = dto.StudCertificateCount,
                StudPostgradCount = dto.StudPostgradCount,

                GraduatesTotal = dto.GraduatesTotal,
                AlumniCount = dto.AlumniCount,

                OpSustainabilityPct = dto.OpSustainabilityPct,
                EmployabilityPct = dto.EmployabilityPct,
                PhdStaffPct = dto.PhdStaffPct,
                FteRatio = dto.FteRatio,
                TeachingLoadHours = dto.TeachingLoadHours,

                AnnualPublications = dto.AnnualPublications,
                OnlineProgramsCount = dto.OnlineProgramsCount,
                IntlAccreditedProgramsCount = dto.IntlAccreditedProgramsCount,

                ExternalGrants = dto.ExternalGrants,
                Notes = dto.Notes,

                AccreditationStatus = dto.AccreditationStatus,
                AccreditationBy = dto.AccreditationBy,
                AccreditationDate = dto.AccreditationDate,
                CommitteeComment = dto.CommitteeComment,

                IsDraft = dto.IsDraft,
                IsActive = dto.IsActive,

                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.UnUniversityRegistrations.Add(entity);
            await _context.SaveChangesAsync();

            return entity.RegistrationId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UniversityRequestDto dto)
        {
            if (dto.RegistrationId == null || dto.RegistrationId == 0)
                return false;

            if (await _context.UnUniversityRegistrations.AnyAsync(x =>
                      x.UniversityName.ToLower() == dto.UniversityName.ToLower()
                      && x.RegistrationId != dto.RegistrationId))
            {
                throw new CustomException("University with same name already exists");
            }

            var entity = await _context.UnUniversityRegistrations
                .FirstOrDefaultAsync(x => x.RegistrationId == dto.RegistrationId);

            if (entity == null)
                return false;

            entity.UniversityName = dto.UniversityName;
            entity.UniversityType = dto.UniversityTypeId;

            entity.CharterAccreditation = dto.CharterAccreditation;
            entity.EstablishedYear = dto.EstablishedYear;

            entity.CountryId = dto.CountryId;
            entity.City = dto.City;
            entity.Address = dto.Address;
            entity.Website = dto.Website;

            entity.VcName = dto.VcName;
            entity.VcEmail = dto.VcEmail;
            entity.VcMobile = dto.VcMobile;

            entity.CoordName = dto.CoordName;
            entity.CoordPosition = dto.CoordPosition;
            entity.CoordEmail = dto.CoordEmail;
            entity.CoordPhone = dto.CoordPhone;

            entity.FacultiesCount = dto.FacultiesCount;
            entity.FacultyFulltimeCount = dto.FacultyFulltimeCount;
            entity.AdminStaffCount = dto.AdminStaffCount;

            entity.ProgDegreeCount = dto.ProgDegreeCount;
            entity.ProgDiplomaCount = dto.ProgDiplomaCount;
            entity.ProgCertificateCount = dto.ProgCertificateCount;
            entity.ProgPostgradCount = dto.ProgPostgradCount;

            entity.StudentsTotal = dto.StudentsTotal;
            entity.StudentsEnrolled = dto.StudentsEnrolled;
            entity.IntlStudentsPct = dto.IntlStudentsPct;
            entity.StudentsGenderTypeId = dto.StudentsGenderTypeId;

            entity.StudDegreeCount = dto.StudDegreeCount;
            entity.StudDiplomaCount = dto.StudDiplomaCount;
            entity.StudCertificateCount = dto.StudCertificateCount;
            entity.StudPostgradCount = dto.StudPostgradCount;

            entity.GraduatesTotal = dto.GraduatesTotal;
            entity.AlumniCount = dto.AlumniCount;

            entity.OpSustainabilityPct = dto.OpSustainabilityPct;
            entity.EmployabilityPct = dto.EmployabilityPct;
            entity.PhdStaffPct = dto.PhdStaffPct;
            entity.FteRatio = dto.FteRatio;
            entity.TeachingLoadHours = dto.TeachingLoadHours;

            entity.AnnualPublications = dto.AnnualPublications;
            entity.OnlineProgramsCount = dto.OnlineProgramsCount;
            entity.IntlAccreditedProgramsCount = dto.IntlAccreditedProgramsCount;

            entity.ExternalGrants = dto.ExternalGrants;
            entity.Notes = dto.Notes;

            entity.AccreditationStatus = dto.AccreditationStatus;
            entity.AccreditationBy = dto.AccreditationBy;
            entity.AccreditationDate = dto.AccreditationDate;
            entity.CommitteeComment = dto.CommitteeComment;

            entity.IsDraft = dto.IsDraft;
            entity.IsActive = dto.IsActive;

            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }



        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UnUniversityRegistrations
                .FirstOrDefaultAsync(x => x.RegistrationId == id);

            if (entity == null)
                return false;

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<UniversityRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UnUniversityRegistrations
                .AsNoTracking()
                .Where(x => x.RegistrationId == id)
                .Select(x => new UniversityRequestDto
                {
                    RegistrationId = x.RegistrationId,

                    UniversityName = x.UniversityName,
                    UniversityTypeId = x.UniversityType,

                    CharterAccreditation = x.CharterAccreditation,
                    EstablishedYear = x.EstablishedYear,

                    CountryId = x.CountryId,
                    City = x.City,
                    Address = x.Address,
                    Website = x.Website,

                    VcName = x.VcName,
                    VcEmail = x.VcEmail,
                    VcMobile = x.VcMobile,

                    CoordName = x.CoordName,
                    CoordPosition = x.CoordPosition,
                    CoordEmail = x.CoordEmail,
                    CoordPhone = x.CoordPhone,

                    FacultiesCount = x.FacultiesCount,
                    FacultyFulltimeCount = x.FacultyFulltimeCount,
                    AdminStaffCount = x.AdminStaffCount,

                    ProgDegreeCount = x.ProgDegreeCount,
                    ProgDiplomaCount = x.ProgDiplomaCount,
                    ProgCertificateCount = x.ProgCertificateCount,
                    ProgPostgradCount = x.ProgPostgradCount,

                    StudentsTotal = x.StudentsTotal,
                    StudentsEnrolled = x.StudentsEnrolled,
                    IntlStudentsPct = x.IntlStudentsPct,

                    StudentsGenderTypeId = x.StudentsGenderTypeId,

                    StudDegreeCount = x.StudDegreeCount,
                    StudDiplomaCount = x.StudDiplomaCount,
                    StudCertificateCount = x.StudCertificateCount,
                    StudPostgradCount = x.StudPostgradCount,

                    GraduatesTotal = x.GraduatesTotal,
                    AlumniCount = x.AlumniCount,

                    OpSustainabilityPct = x.OpSustainabilityPct,
                    EmployabilityPct = x.EmployabilityPct,
                    PhdStaffPct = x.PhdStaffPct,
                    FteRatio = x.FteRatio,
                    TeachingLoadHours = x.TeachingLoadHours,

                    AnnualPublications = x.AnnualPublications,
                    OnlineProgramsCount = x.OnlineProgramsCount,
                    IntlAccreditedProgramsCount = x.IntlAccreditedProgramsCount,

                    ExternalGrants = x.ExternalGrants,
                    Notes = x.Notes,

                    AccreditationStatus = x.AccreditationStatus,
                    AccreditationBy = x.AccreditationBy,
                    AccreditationDate = x.AccreditationDate,
                    CommitteeComment = x.CommitteeComment,

                    IsDraft = x.IsDraft,
                    IsActive = x.IsActive,

                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate,

                    // Navigation Properties
                    CountryName = x.Country != null
                        ? x.Country.CountryName
                        : null,

                    UniversityTypeName = x.UniversityTypeNavigation != null
                        ? x.UniversityTypeNavigation.DisplayText
                        : null,

                    StudentsGenderTypeName = x.StudentsGenderType != null
                        ? x.StudentsGenderType.DisplayText
                        : null,

                    AccreditationByName = x.AccreditationByNavigation != null
                        ? x.AccreditationByNavigation.LoginName
                        : null,

                    CreatedByName = x.CreatedByNavigation != null
                        ? x.CreatedByNavigation.LoginName
                        : null,

                    UpdatedByName = x.UpdatedByNavigation != null
                        ? x.UpdatedByNavigation.LoginName
                        : null,

                    FormattedCreatedDate = x.CreatedDate.ToString("dd MMM yyyy"),
                    FormattedUpdatedDate = x.UpdatedDate.HasValue
                        ? x.UpdatedDate.Value.ToString("dd MMM yyyy")
                        : null
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UniversityRequestDto>> GetByFilterAsync(UniversityFilterDto filter)
        {
            var query = _context.UnUniversityRegistrations
                .AsNoTracking()
                .AsQueryable();

            if (filter.RegistrationId.HasValue)
                query = query.Where(x => x.RegistrationId == filter.RegistrationId);

            if (!string.IsNullOrWhiteSpace(filter.UniversityName))
                query = query.Where(x => x.UniversityName.Contains(filter.UniversityName));

            if (filter.CountryId.HasValue)
                query = query.Where(x => x.CountryId == filter.CountryId);

            if (filter.UniversityTypeId.HasValue)
                query = query.Where(x => x.UniversityType == filter.UniversityTypeId);

            if (filter.StudentsGenderTypeId.HasValue)
                query = query.Where(x => x.StudentsGenderTypeId == filter.StudentsGenderTypeId);

            if (filter.AccreditationStatus.HasValue)
                query = query.Where(x => x.AccreditationStatus == filter.AccreditationStatus);

            if (filter.AccreditationBy.HasValue)
                query = query.Where(x => x.AccreditationBy == filter.AccreditationBy);

            if (filter.IsDraft.HasValue)
                query = query.Where(x => x.IsDraft == filter.IsDraft);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);

            if (filter.CreatedFrom.HasValue)
                query = query.Where(x => x.CreatedDate >= filter.CreatedFrom);

            if (filter.CreatedTo.HasValue)
                query = query.Where(x => x.CreatedDate <= filter.CreatedTo);

            if (filter.AccreditationFrom.HasValue)
                query = query.Where(x => x.AccreditationDate >= filter.AccreditationFrom);

            if (filter.AccreditationTo.HasValue)
                query = query.Where(x => x.AccreditationDate <= filter.AccreditationTo);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.UniversityName.ToLower().Contains(search) ||
                    x.City.ToLower().Contains(search) ||
                    x.CoordName.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(x => x.RegistrationId);

            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UniversityRequestDto
                {
                    RegistrationId = x.RegistrationId,

                    UniversityName = x.UniversityName,

                    UniversityTypeId = x.UniversityType,
                    UniversityTypeName = x.UniversityTypeNavigation != null
                        ? x.UniversityTypeNavigation.DisplayText
                        : null,

                    CharterAccreditation = x.CharterAccreditation,

                    EstablishedYear = x.EstablishedYear,

                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,

                    City = x.City,
                    Address = x.Address,
                    Website = x.Website,

                    CoordName = x.CoordName,
                    CoordEmail = x.CoordEmail,
                    CoordPhone = x.CoordPhone,

                    StudentsGenderTypeId = x.StudentsGenderTypeId,
                    StudentsGenderTypeName = x.StudentsGenderType != null
                        ? x.StudentsGenderType.DisplayText
                        : null,

                    StudentsTotal = x.StudentsTotal,
                    StudentsEnrolled = x.StudentsEnrolled,

                    AccreditationStatus = x.AccreditationStatus,

                    AccreditationBy = x.AccreditationBy,
                    AccreditationByName = x.AccreditationByNavigation != null
                        ? x.AccreditationByNavigation.LoginName
                        : null,

                    IsDraft = x.IsDraft,
                    IsActive = x.IsActive,

                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation != null
                        ? x.CreatedByNavigation.LoginName
                        : null,

                    CreatedDate = x.CreatedDate,
                    FormattedCreatedDate = x.CreatedDate.ToString("dd MMM yyyy")
                })
                .ToListAsync();

            return new PagedResultDto<UniversityRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
