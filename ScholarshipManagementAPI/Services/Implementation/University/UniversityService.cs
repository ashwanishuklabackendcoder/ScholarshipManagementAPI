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
                IsActive = dto.IsActive,
                CountryId = dto.CountryId,
                ApprovalStatus = dto.IsApproved ? 1 : 0,
                ApprovedBy = dto.ApprovedBy,
                Notes = dto.Remarks,
                City = "", 
                CoordName = "",
                CoordEmail = "",
                CoordPhone = "",

                CreatedDate = DateTime.UtcNow,     // always server-side
            };

            _context.UnUniversityRegistrations.Add(entity);
            await _context.SaveChangesAsync();

            return entity.RegistrationId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UniversityRequestDto dto)
        {
            if (dto.UniversityId == null || dto.UniversityId == 0)
                return false;

            if (await _context.UnUniversityRegistrations.AnyAsync(x =>
                      x.UniversityName.ToLower() == dto.UniversityName.ToLower()
                      && x.RegistrationId != dto.UniversityId))
            {
                throw new CustomException("University with same name already exists");
            }

            var entity = await _context.UnUniversityRegistrations
                .FirstOrDefaultAsync(x => x.RegistrationId == dto.UniversityId);

            if (entity == null)
                return false;

            entity.UniversityName = dto.UniversityName;
            entity.IsActive = dto.IsActive;
            entity.CountryId = dto.CountryId;
            entity.Notes = dto.Remarks;

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
                    UniversityId = x.RegistrationId,
                    UniversityName = x.UniversityName,
                    IsActive = x.IsActive,
                    CountryId = x.CountryId,
                    Remarks = x.Notes,
                    IsApproved = x.ApprovalStatus == 1,
                    ApprovedBy = x.ApprovedBy,

                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ApprovedByName = null,

                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = 0,
                })
                .FirstOrDefaultAsync();
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UniversityRegistrationDto>> GetByFilterAsync(UniversityFilterDto filter)
        {
            var query = _context.UnUniversityRegistrations
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

            // filter date range filter

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.UniversityName.ToLower().Contains(search) 
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.RegistrationId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
               .Select(x => new UniversityRegistrationDto
               {
                   RegistrationId = x.RegistrationId,
                   UniversityName = x.UniversityName,
                   UniversityType = x.UniversityType.HasValue ? x.UniversityType.Value.ToString() : null,
                   CharterAccreditation = x.CharterAccreditation,
                   EstablishedYear = x.EstablishedYear,
                   CountryId = x.CountryId,
                   CountryName = x.Country.CountryName,
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
                   StudentsGender = x.StudentsGender,
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
                   ApprovalStatus = x.ApprovalStatus,
                   ApprovedBy = x.ApprovedBy,
                   CreatedDate = x.CreatedDate
               })
                .ToListAsync();

            return new PagedResultDto<UniversityRegistrationDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }





    }
}
