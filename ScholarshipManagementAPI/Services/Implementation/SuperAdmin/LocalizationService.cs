using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Localization;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class LocalizationService : ILocalizationService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        private const string CacheKeyPrefix = "localization:";

        public LocalizationService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }



        // ---------------- GET TRANSLATIONS ----------------
        public async Task<LocalizationDto?> GetTranslationsAsync(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                return null;

            languageCode = languageCode.Trim().ToUpper();

            var cacheKey = $"{CacheKeyPrefix}{languageCode}";

            // ---------- CACHE ----------
            if (_cache.TryGetValue(cacheKey, out LocalizationDto? cachedData))
            {
                return cachedData;
            }


            // ---------- LANGUAGE ----------
            var language = await _context.ZzLanguages
                .AsNoTracking()
                .Where(x =>
                    x.LanguageCode.ToLower() == languageCode.ToLower() &&
                    x.IsActive)
                .Select(x => new
                {
                    x.LanguageId,
                    x.LanguageCode,
                    x.CultureCode,
                    x.IsRtl
                })
                .FirstOrDefaultAsync();

            if (language == null)
                return null;

            // ---------- LABELS ----------
            var labels = await _context.ZzLabels
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    x.LabelId,
                    x.LabelKey,
                    x.LabelValue
                })
                .ToListAsync();


            // ---------- TRANSLATIONS ----------
            var translations = await _context.ZzLanguageTranslations
                .AsNoTracking()
                .Where(x =>
                    x.LanguageId == language.LanguageId &&
                    x.IsActive)
                .Select(x => new
                {
                    x.LabelId,
                    x.LabelValue
                })
                .ToListAsync();

            var translationLookup = translations
                .GroupBy(x => x.LabelId)
                .ToDictionary(
                    x => x.Key,
                    x => x.First().LabelValue);


            // ---------- BUILD DICTIONARY ----------
            var dictionary = new Dictionary<string, string>();

            foreach (var label in labels)
            {
                // English/default value from zz_labels
                var value = label.LabelValue;

                // Use translated value when available
                if (translationLookup.TryGetValue(label.LabelId, out var translatedValue)
                    && !string.IsNullOrWhiteSpace(translatedValue))
                {
                    value = translatedValue;
                }

                dictionary[label.LabelKey] = value;
            }

            var result = new LocalizationDto
            {
                LanguageId = language.LanguageId,
                LanguageCode = language.LanguageCode,
                CultureCode = language.CultureCode,
                IsRTL = language.IsRtl,
                Translations = dictionary
            };

            // ---------- STORE CACHE ----------
            _cache.Set(
                cacheKey,
                result,
                new MemoryCacheEntryOptions
                {
                    // Expire after 6 hours of inactivity.
                    SlidingExpiration = TimeSpan.FromHours(6),

                    // Hard limit: expire after 24 hours regardless of activity.
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                });

            return result;
        }


        // ---------------- CLEAR LANGUAGE CACHE ----------------
        public void ClearLanguageCache(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                return;

            var cacheKey = $"{CacheKeyPrefix}{languageCode.Trim().ToUpper()}";

            _cache.Remove(cacheKey);
        }


        // ---------------- CLEAR ALL CACHE ----------------
        public async Task ClearAllLocalizationCacheAsync()
        {
            var languageCodes = await _context.ZzLanguages
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => x.LanguageCode)
                .ToListAsync();

            foreach (var languageCode in languageCodes)
            {
                ClearLanguageCache(languageCode);
            }
        }

    }
}
