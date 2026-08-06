using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.ProgramRegistrationWindow;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IProgramRegistrationWindowService
    {

        Task<ProgramRegistrationWindowRequestDto?> GetByProgramIdAsync(long programId, LoggedInUserDto currentUser);

        Task<long> SaveAsync(ProgramRegistrationWindowRequestDto dto, LoggedInUserDto currentUser);


    }

}
