using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.Donor;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IDonorService
    {
        Task<long> CreateAsync(DonorRequestDto dto);
        Task<bool> UpdateAsync(DonorRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<DonorRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<DonorRequestDto>> GetByFilterAsync(DonorFilterDto filter);
    }
}
