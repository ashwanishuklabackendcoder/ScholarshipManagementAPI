using ScholarshipManagementAPI.DTOs.SuperAdmin.Localization;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface ILocalizationService
    {
        Task<LocalizationDto?> GetTranslationsAsync(string languageCode);

        void ClearLanguageCache(string languageCode);

        Task ClearAllLocalizationCacheAsync();
    }
}
