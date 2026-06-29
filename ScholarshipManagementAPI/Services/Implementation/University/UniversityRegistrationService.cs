using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class UniversityRegistrationService : IUniversityRegistrationService
    {
        private readonly AppDbContext _context;

        public UniversityRegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<long> RegisterAsync(UniversityRegistrationDto dto)
        {
            // Validate unique name in pending or active list
            if (await _context.UnUniversityRegistrations.AnyAsync(x => x.UniversityName.ToLower() == dto.UniversityName.ToLower() && x.ApprovalStatus == 0))
            {
                throw new CustomException("A pending registration for this university already exists.");
            }
            if (await _context.UnUniversityLists.AnyAsync(x => x.UniversityName.ToLower() == dto.UniversityName.ToLower()))
            {
                throw new CustomException("This university is already accredited and active.");
            }

            var entity = new UnUniversityRegistration
            {
                UniversityName = dto.UniversityName,
                UniversityType = dto.UniversityType,
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
                StudentsGender = dto.StudentsGender,

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
                ApprovalStatus = 0, // Pending
                ApprovedBy = null,

                IsDraft = true,
                IsActive = true,
                CreatedBy = 2, // Anonymous self-registration defaults to admin login
                CreatedDate = DateTime.UtcNow
            };

            _context.UnUniversityRegistrations.Add(entity);
            await _context.SaveChangesAsync();

            return entity.RegistrationId;
        }

        public async Task<PagedResultDto<UniversityRegistrationDto>> GetRegistrationsByStatusAsync(int approvalStatus, int pageNumber, int pageSize)
        {
            var query = _context.UnUniversityRegistrations
                .Include(x => x.Country)
                .AsNoTracking()
                .Where(x => x.ApprovalStatus == approvalStatus)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UniversityRegistrationDto
                {
                    RegistrationId = x.RegistrationId,
                    UniversityName = x.UniversityName,
                    UniversityType = x.UniversityType,
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
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> ApproveRegistrationAsync(long id, int status, long approvedByUserId)
        {
            var reg = await _context.UnUniversityRegistrations
                .FirstOrDefaultAsync(x => x.RegistrationId == id);

            if (reg == null) return false;

            if (reg.ApprovalStatus != 0)
            {
                throw new CustomException("This registration has already been processed.");
            }

            reg.ApprovalStatus = status;
            reg.ApprovedBy = approvedByUserId;
            reg.UpdatedBy = approvedByUserId;
            reg.UpdatedDate = DateTime.UtcNow;

            // If approved (status = 1), promote/insert to UnUniversityList
            if (status == 1)
            {
                var uni = new UnUniversityList
                {
                    UniversityName = reg.UniversityName,
                    CountryId = reg.CountryId,
                    IsActive = true,
                    IsApproved = true,
                    ApprovedBy = null, // Can map to HrStaffMaster if needed, otherwise leave null or use fallback
                    CreatedDate = DateTime.UtcNow,
                    Remarks = $"Self-Registration Approved by User {approvedByUserId} on {DateTime.UtcNow:yyyy-MM-dd}"
                };
                _context.UnUniversityLists.Add(uni);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
