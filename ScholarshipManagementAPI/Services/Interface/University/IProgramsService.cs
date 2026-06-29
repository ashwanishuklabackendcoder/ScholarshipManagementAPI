using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Programs;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IProgramsService
    {
        Task<long> CreateAsync(ProgramRequestDto dto);

        Task<bool> UpdateAsync(ProgramRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<ProgramRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<ProgramRequestDto>> GetByFilterAsync(ProgramFilterDto filter);
    }
}
