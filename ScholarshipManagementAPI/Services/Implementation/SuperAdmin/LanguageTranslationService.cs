using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class LanguageTranslationService : ILanguageTranslationService
    {
        private readonly AppDbContext _context;
        private readonly ILocalizationService _localizationService;
        
        
        public LanguageTranslationService(AppDbContext context, ILocalizationService localizationService)
        {
            _context = context;
            _localizationService = localizationService;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(LanguageTranslationRequestDto dto)
        {
            // Make sure label exists
            if (!await _context.ZzLabels
                .AnyAsync(x => x.LabelId == dto.LabelId && x.IsActive))
            {
                throw new CustomException("Label not found");
            }

            // Make sure language exists
            var language = await _context.ZzLanguages
                .AsNoTracking()
                .Where(x => x.LanguageId == dto.LanguageId && x.IsActive)
                .Select(x => new
                {
                    x.LanguageId,
                    x.LanguageCode
                })
                .FirstOrDefaultAsync();

            if (language == null)
            {
                throw new CustomException("Language not found");
            }

            // One translation per Label + Language
            if (await _context.ZzLanguageTranslations.AnyAsync(x =>
                x.LabelId == dto.LabelId &&
                x.LanguageId == dto.LanguageId))
            {
                throw new CustomException(
                    "Translation already exists for this label and language");
            }

            var entity = new ZzLanguageTranslation
            {
                LabelId = dto.LabelId,
                LanguageId = dto.LanguageId,
                LabelValue = dto.LabelValue,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.ZzLanguageTranslations.Add(entity);

            await _context.SaveChangesAsync();

            // Clear the cache for the specific language
            if (!string.IsNullOrWhiteSpace(language.LanguageCode))
            {
                _localizationService.ClearLanguageCache(language.LanguageCode);
            }

            return entity.TranslationId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(LanguageTranslationRequestDto dto)
        {
            if (dto.TranslationId == null || dto.TranslationId == 0)
                return false;

            var entity = await _context.ZzLanguageTranslations
                .FirstOrDefaultAsync(x =>
                    x.TranslationId == dto.TranslationId);

            if (entity == null)
                return false;

            // Do not allow changing LabelId / LanguageId
            // because TranslationId already belongs to that combination.

            entity.LabelValue = dto.LabelValue;
            entity.IsActive = true;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            var languageCode = await _context.ZzLanguages
                .Where(x => x.LanguageId == entity.LanguageId)
                .Select(x => x.LanguageCode)
                .FirstOrDefaultAsync();

            await _context.SaveChangesAsync();

            // Clear the cache for the specific language
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                _localizationService.ClearLanguageCache(languageCode);
            }

            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzLanguageTranslations
                .FirstOrDefaultAsync(x =>
                    x.TranslationId == id);

            if (entity == null)
                return false;

            var languageCode = await _context.ZzLanguages
                .Where(x => x.LanguageId == entity.LanguageId)
                .Select(x => x.LanguageCode)
                .FirstOrDefaultAsync();

            entity.IsActive = false;

            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                _localizationService.ClearLanguageCache(languageCode);
            }

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<LanguageTranslationRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzLanguageTranslations
                .AsNoTracking()
                .Where(x =>
                    x.TranslationId == id &&
                    x.IsActive)
                .Select(x => new LanguageTranslationRequestDto
                {
                    TranslationId = x.TranslationId,

                    LabelId = x.LabelId,
                    LanguageId = x.LanguageId,

                    LabelValue = x.LabelValue,
                    LabelKey = x.Label != null ? x.Label.LabelKey : null,
                    LanguageName = x.Language != null ? x.Language.LanguageName : null,
                    LanguageCode = x.Language != null ? x.Language.LanguageCode : null,


                    IsActive = x.IsActive,

                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,

                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,

                    CreatedByName = x.CreatedByNavigation != null
                        ? x.CreatedByNavigation.Staff.StaffSalutation + " " +
                          x.CreatedByNavigation.Staff.StaffFirstName + " " +
                          x.CreatedByNavigation.Staff.StaffLastName
                        : null,

                    UpdatedByName = x.UpdatedByNavigation != null
                        ? x.UpdatedByNavigation.Staff.StaffSalutation + " " +
                          x.UpdatedByNavigation.Staff.StaffFirstName + " " +
                          x.UpdatedByNavigation.Staff.StaffLastName
                        : null
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<LanguageTranslationRequestDto>>GetByFilterAsync(LanguageTranslationFilterDto filter)
        {
            var query = _context.ZzLanguageTranslations
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (filter.LabelId.HasValue)
            {
                query = query.Where(x => x.LabelId == filter.LabelId.Value);
            }

            if (filter.LanguageId.HasValue)
            {
                query = query.Where(x => x.LanguageId == filter.LanguageId.Value);
            }


            // Global Search
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.LabelValue.ToLower().Contains(search) ||
                    (x.Label != null && x.Label.LabelKey.ToLower().Contains(search)) ||
                    (x.Language != null && x.Language.LanguageName.ToLower().Contains(search))
                );
            }

            // ---------- Total Count ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query
                .OrderBy(x => x.LabelId)
                .ThenBy(x => x.LanguageId);

            // ---------- Pagination ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new LanguageTranslationRequestDto
                {
                    TranslationId = x.TranslationId,

                    LabelId = x.LabelId,
                    LanguageId = x.LanguageId,

                    LabelValue = x.LabelValue,
                    LabelKey = x.Label != null ? x.Label.LabelKey: null,
                    LanguageName = x.Language != null ? x.Language.LanguageName : null,
                    LanguageCode = x.Language != null ? x.Language.LanguageCode : null,

                    IsActive = x.IsActive,

                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,

                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,

                    CreatedByName = x.CreatedByNavigation != null
                        ? x.CreatedByNavigation.Staff.StaffSalutation + " " +
                          x.CreatedByNavigation.Staff.StaffFirstName + " " +
                          x.CreatedByNavigation.Staff.StaffLastName
                        : null,

                    UpdatedByName = x.UpdatedByNavigation != null
                        ? x.UpdatedByNavigation.Staff.StaffSalutation + " " +
                          x.UpdatedByNavigation.Staff.StaffFirstName + " " +
                          x.UpdatedByNavigation.Staff.StaffLastName
                        : null
                })
                .ToListAsync();

            return new PagedResultDto<LanguageTranslationRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }





        // ---------------- TRANSLATION MANAGEMENT ----------------
        public async Task<PagedResultDto<LanguageTranslationManagementDto>>GetManagementAsync(LanguageTranslationFilterDto filter)
        {
            var labelsQuery = _context.ZzLabels
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            // ---------- Module Filter ----------
            if (filter.ModuleId.HasValue)
            {
                labelsQuery = labelsQuery
                    .Where(x => x.ModuleId == filter.ModuleId.Value);
            }

            // ---------- Label Filter ----------
            if (filter.LabelId.HasValue)
            {
                labelsQuery = labelsQuery
                    .Where(x => x.LabelId == filter.LabelId.Value);
            }

            // ---------- Global Search ----------
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                labelsQuery = labelsQuery.Where(x =>
                    x.LabelKey.ToLower().Contains(search) ||
                    x.LabelValue.ToLower().Contains(search) ||
                    (x.Module != null &&
                     x.Module.ModuleName.ToLower().Contains(search))
                );
            }

            // ---------- Total Count ----------
            var totalCount = await labelsQuery.CountAsync();

            // ---------- Ordering ----------
            labelsQuery = labelsQuery
                .OrderBy(x => x.ModuleId)
                .ThenBy(x => x.LabelKey);

            // ---------- Pagination ----------
            if (filter.PageSize > 0)
            {
                labelsQuery = labelsQuery
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var labels = await labelsQuery
                .Select(x => new
                {
                    x.LabelId,
                    x.ModuleId,
                    ModuleName = x.Module != null ? x.Module.ModuleName : null,
                    x.LabelKey,
                    x.LabelValue
                })
                .ToListAsync();

            // ---------- Active Languages ----------
            var languages = await _context.ZzLanguages
                .AsNoTracking()
                .Where(x => x.IsActive && !x.IsDefault)
                .Select(x => new
                {
                    x.LanguageId,
                    x.LanguageName,
                    x.LanguageCode
                })
                .ToListAsync();

            var labelIds = labels.Select(x => x.LabelId).ToList();

            // ---------- Existing Translations ----------
            var translations = await _context.ZzLanguageTranslations
                .AsNoTracking()
                .Where(x => x.IsActive && labelIds.Contains(x.LabelId))
                .Select(x => new
                {
                    x.TranslationId,
                    x.LabelId,
                    x.LanguageId,
                    x.LabelValue
                })
                .ToListAsync();

            // ---------- Build Management Result ----------
            var items = labels.Select(label => new LanguageTranslationManagementDto
            {
                LabelId = label.LabelId,
                ModuleId = label.ModuleId,
                ModuleName = label.ModuleName,
                LabelKey = label.LabelKey,
                EnglishMasterValue = label.LabelValue,

                Translations = languages
                    .Select(language =>
                    {
                        var translation = translations.FirstOrDefault(x =>
                            x.LabelId == label.LabelId &&
                            x.LanguageId == language.LanguageId);

                        return new LanguageTranslationItemDto
                        {
                            LanguageId = language.LanguageId,
                            LanguageName = language.LanguageName,
                            LanguageCode = language.LanguageCode,
                            Value = translation?.LabelValue,
                            IsTranslated = translation != null
                        };
                    })
                    .ToList()
            }).ToList();

            return new PagedResultDto<LanguageTranslationManagementDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



    }
}
