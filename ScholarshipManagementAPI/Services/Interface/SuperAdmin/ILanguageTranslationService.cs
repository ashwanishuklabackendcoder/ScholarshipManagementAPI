using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface ILanguageTranslationService
    {
        Task<long> CreateAsync(LanguageTranslationRequestDto dto);

        Task<bool> UpdateAsync(LanguageTranslationRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<LanguageTranslationRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<LanguageTranslationRequestDto>>GetByFilterAsync(LanguageTranslationFilterDto filter);



        // ---------------- MANAGEMENT ----------------
        Task<PagedResultDto<LanguageTranslationManagementDto>>GetManagementAsync(LanguageTranslationFilterDto filter);



    }
}
