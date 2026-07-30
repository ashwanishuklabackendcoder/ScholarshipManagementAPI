using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.ProgramRegistrationWindow;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IProgramRegistrationWindowService
    {

        Task<ProgramRegistrationWindowRequestDto?> GetByProgramIdAsync(long programId);

        Task<long> SaveAsync(ProgramRegistrationWindowRequestDto dto, long loginId);


    }

}
