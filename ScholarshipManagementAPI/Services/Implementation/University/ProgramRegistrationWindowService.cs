using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.ProgramRegistrationWindow;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class ProgramRegistrationWindowService : IProgramRegistrationWindowService
    {
        private readonly AppDbContext _context;
        
        public ProgramRegistrationWindowService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProgramRegistrationWindowRequestDto?> GetByProgramIdAsync(long programId, LoggedInUserDto currentUser)
        {
            if (currentUser.StaffType != StaffType.University)
                throw new UnauthorizedAccessException();

            if (!currentUser.UniversityIds.Any())
                throw new UnauthorizedAccessException("User is not associated with a university.");


            var entity = await _context.KfProgramRegistrationWindows
                .AsNoTracking()
                .Include(x => x.Program)
                .Include(x => x.CreatedByNavigation)
                    .ThenInclude(x => x.Staff)
                .Include(x => x.UpdatedByNavigation)
                    .ThenInclude(x => x.Staff)
                .FirstOrDefaultAsync(x =>
                    x.ProgramId == programId &&
                    currentUser.UniversityIds.Contains(x.Program.UniversityId));

            if (entity == null)
                return null;

            return new ProgramRegistrationWindowRequestDto
            {
                Id = entity.Id,
                ProgramId = entity.ProgramId,
                SemesterNo = entity.SemesterNo,
                RegistrationFrom = entity.RegistrationFrom,
                RegistrationTo = entity.RegistrationTo,
                Notes = entity.Notes,

                ProgramName = entity.Program.ProgramName,

                CreatedByName = UserDisplayHelper.GetFullName(
                    entity.CreatedByNavigation.Staff.StaffSalutation,
                    entity.CreatedByNavigation.Staff.StaffFirstName,
                    entity.CreatedByNavigation.Staff.StaffLastName),

                UpdatedByName = entity.UpdatedByNavigation?.Staff != null
                    ? UserDisplayHelper.GetFullName(
                        entity.UpdatedByNavigation.Staff.StaffSalutation,
                        entity.UpdatedByNavigation.Staff.StaffFirstName,
                        entity.UpdatedByNavigation.Staff.StaffLastName)
                    : null
            };
        }


        public async Task<long> SaveAsync(ProgramRegistrationWindowRequestDto dto, LoggedInUserDto currentUser)
        {

            if (currentUser.StaffType != StaffType.University)
                throw new UnauthorizedAccessException();

            if (!currentUser.UniversityIds.Any())
                throw new UnauthorizedAccessException("User is not associated with a university.");


            if (dto.RegistrationFrom >= dto.RegistrationTo)
                throw new CustomException("Registration From must be earlier than Registration To.");

            var program = await _context.KfPrograms
                .FirstOrDefaultAsync(x =>
                    x.ProgramId == dto.ProgramId &&
                    currentUser.UniversityIds.Contains(x.UniversityId));

            if (program == null)
                throw new UnauthorizedAccessException();

            if (dto.SemesterNo < 1 || dto.SemesterNo > program.NumberOfSemesters)
                throw new CustomException("Invalid semester selected.");

            var entity = await _context.KfProgramRegistrationWindows
                .FirstOrDefaultAsync(x =>
                    x.ProgramId == dto.ProgramId &&
                    x.SemesterNo == dto.SemesterNo);

            if (entity == null)
            {
                entity = new KfProgramRegistrationWindow
                {
                    ProgramId = dto.ProgramId,
                    SemesterNo = dto.SemesterNo,
                    RegistrationFrom = dto.RegistrationFrom,
                    RegistrationTo = dto.RegistrationTo,
                    Notes = dto.Notes,

                    CreatedBy = currentUser.LoginId,
                    CreatedOn = DateTime.UtcNow
                };

                _context.KfProgramRegistrationWindows.Add(entity);
            }
            else
            {
                entity.RegistrationFrom = dto.RegistrationFrom;
                entity.RegistrationTo = dto.RegistrationTo;
                entity.Notes = dto.Notes;

                entity.UpdatedBy = currentUser.LoginId;
                entity.UpdatedOn = DateTime.UtcNow;

                _context.KfProgramRegistrationWindows.Update(entity);
            }

            await _context.SaveChangesAsync();

            return entity.Id;
        }


    }
}
