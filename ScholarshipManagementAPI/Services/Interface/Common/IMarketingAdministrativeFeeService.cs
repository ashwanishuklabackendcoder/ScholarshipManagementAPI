using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.MarketingAdministrativeFee;

namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface IMarketingAdministrativeFeeService
    {
        Task<MarketingAdministrativeFeeResponseDto> GetCurrentAsync();

        Task<bool> UpdateAsync(MarketingAdministrativeFeeRequestDto dto, LoggedInUserDto currentUser);

        Task<List<MarketingAdministrativeFeeHistoryDto>> GetHistoryAsync();

    }
}
