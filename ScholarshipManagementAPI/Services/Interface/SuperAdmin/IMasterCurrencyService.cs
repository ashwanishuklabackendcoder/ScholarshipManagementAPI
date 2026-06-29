using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IMasterCurrencyService
    {
        Task<long> CreateAsync(MasterCurrencyRequestDto dto);
        Task<bool> UpdateAsync(MasterCurrencyRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<MasterCurrencyRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<MasterCurrencyRequestDto>> GetByFilterAsync(MasterCurrencyFilterDto filter);
    }
}
