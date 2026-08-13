using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Languages;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface ILanguageService
    {
        Task<long> CreateAsync(LanguageRequestDto dto);

        Task<bool> UpdateAsync(LanguageRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<LanguageRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<LanguageRequestDto>> GetByFilterAsync(LanguageFilterDto filter);
    }
}
