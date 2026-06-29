using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface IGeneralSettingsService
    {
        Task<long> CreateAsync(GeneralSettingRequestDto dto);
        Task<bool> UpdateAsync(GeneralSettingRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<GeneralSettingRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<GeneralSettingRequestDto>> GetByFilterAsync(GeneralSettingFilterDto filter);




        // ADD THESE (Important)
        Task<GeneralConfigDto> GetGeneralConfigAsync();


    }
}
