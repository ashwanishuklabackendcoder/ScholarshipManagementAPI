using Amazon.Runtime.Internal.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;
using static ScholarshipManagementAPI.Helper.Utilities.Constant;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class GeneralSettingsService : IGeneralSettingsService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public GeneralSettingsService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(GeneralSettingRequestDto dto)
        {
            if (await _context.ZzGeneralSettings
                .AnyAsync(x => x.ConfigKey.ToLower() == dto.ConfigKey.ToLower()))
            {
                throw new CustomException("Config key already exists");
            }


            var entity = new ZzGeneralSetting
            {
                ConfigKey = dto.ConfigKey,
                ConfigValue = dto.ConfigValue,
                ConfigDescription = dto.ConfigDescription
            };

            _context.ZzGeneralSettings.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ConfigId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(GeneralSettingRequestDto dto)
        {
            if (dto.ConfigId == null || dto.ConfigId == 0)
                return false;

            if (await _context.ZzGeneralSettings.AnyAsync(x =>
                      x.ConfigKey.ToLower() == dto.ConfigKey.ToLower()
                      && x.ConfigId != dto.ConfigId))
            {
                throw new CustomException("Config key already exists");
            }

            var entity = await _context.ZzGeneralSettings
                .FirstOrDefaultAsync(x => x.ConfigId == dto.ConfigId);

            if (entity == null)
                return false;

            entity.ConfigKey = dto.ConfigKey;
            entity.ConfigValue = dto.ConfigValue;
            entity.ConfigDescription = dto.ConfigDescription;
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();


            // Invalidate cache for general config when any setting is updated
            _cache.Remove(CacheKeys.GeneralConfig);

            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzGeneralSettings
                .FirstOrDefaultAsync(x => x.ConfigId == id);

            if (entity == null)
                return false;

            _context.ZzGeneralSettings.Remove(entity);
            await _context.SaveChangesAsync();


            // Invalidate cache for general config when any setting is updated
            _cache.Remove(CacheKeys.GeneralConfig);

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<GeneralSettingRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzGeneralSettings
                .AsNoTracking()
                .Where(x => x.ConfigId == id)
                .Select(x => new GeneralSettingRequestDto
                {
                    ConfigId = x.ConfigId,
                    ConfigKey = x.ConfigKey,
                    ConfigValue = x.ConfigValue,
                    ConfigDescription = x.ConfigDescription
                })
                .FirstOrDefaultAsync();

        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<GeneralSettingRequestDto>> GetByFilterAsync(GeneralSettingFilterDto filter)
        {
            var query = _context.ZzGeneralSettings
                .AsNoTracking()
                .AsQueryable();

            /* Filter by ConfigKey */
            if (!string.IsNullOrWhiteSpace(filter.ConfigKey))
            {
                var key = filter.ConfigKey.Trim().ToLower();
                query = query.Where(x => x.ConfigKey.ToLower().Contains(key));
            }

            /* Filter by ConfigValue */
            if (!string.IsNullOrWhiteSpace(filter.ConfigValue))
            {
                var value = filter.ConfigValue.Trim().ToLower();
                query = query.Where(x => x.ConfigValue.ToLower().Contains(value));
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.ConfigKey.ToLower().Contains(search) ||
                    x.ConfigValue.ToLower().Contains(search) ||
                    (x.ConfigDescription != null && x.ConfigDescription.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.ConfigId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new GeneralSettingRequestDto
                {
                    ConfigId = x.ConfigId,
                    ConfigKey = x.ConfigKey,
                    ConfigValue = x.ConfigValue,
                    ConfigDescription = x.ConfigDescription
                })
                .ToListAsync();

            return new PagedResultDto<GeneralSettingRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



        public async Task<GeneralConfigDto> GetGeneralConfigAsync()
        {
            var result = await _cache.GetOrCreateAsync(CacheKeys.GeneralConfig, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);

                var configs = await _context.ZzGeneralSettings
                    .AsNoTracking()
                    .ToDictionaryAsync(x => x.ConfigKey, x => x.ConfigValue);

                return new GeneralConfigDto
                {
                    FullNameFormat = configs.GetValueOrDefault("FullNameFormat", "FirstName LastName"),
                    DateFormat = configs.GetValueOrDefault("DateFormat", "dd-MM-yyyy"),
                    TimeFormat = configs.GetValueOrDefault("TimeFormat", "HH:mm"),
                    BaseCurrencyName = configs.GetValueOrDefault("BaseCurrencyName", "Rupees"),
                    BaseCurrencySymbol = configs.GetValueOrDefault("BaseCurrencySymbol", "Rs."),
                    BaseCurrencyCode = configs.GetValueOrDefault("BaseCurrencyCode", "INR")
                };
            });

            return result!;
        }


    }
}
