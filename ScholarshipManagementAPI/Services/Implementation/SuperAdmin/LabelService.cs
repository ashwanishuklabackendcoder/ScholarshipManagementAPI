using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Label;
using ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class LabelService : ILabelService
    {
        private readonly AppDbContext _context;

        public LabelService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(LabelRequestDto dto)
        {
            if (await _context.ZzLabels
                .AnyAsync(x => x.LabelName.ToLower() == dto.LabelName.ToLower()))
            {
                throw new CustomException("Label with same name already exists");
            }


            var entity = new ZzLabel
            {
                LabelName = dto.LabelName,
                LabelValue = dto.LabelValue,
                LabelNewValue = dto.LabelNewValue,
                Arabic = dto.Arabic,

                CreatedBy = dto.CreatedBy,        // or from token
                CreatedDate = dto.CreatedDate     // always server-side
            };

            _context.ZzLabels.Add(entity);
            await _context.SaveChangesAsync();

            await IncrementLanguageVersion();

            return entity.LabelId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(LabelRequestDto dto)
        {
            if (dto.LableId == null || dto.LableId == 0)
                return false;

            if (await _context.ZzLabels.AnyAsync(x =>
                      x.LabelName.ToLower() == dto.LabelName.ToLower()
                      && x.LabelId != dto.LableId))
            {
                throw new CustomException("Label with same name already exists");
            }

            var entity = await _context.ZzLabels
                .FirstOrDefaultAsync(x => x.LabelId == dto.LableId);

            if (entity == null)
                return false;

            //entity.LabelName = dto.LabelName; // PK NOT updated

            entity.LabelValue = dto.LabelValue;
            entity.LabelNewValue = dto.LabelNewValue;
            entity.Arabic = dto.Arabic;

            // entity.CreatedBy = dto.CreatedBy;
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();

            await IncrementLanguageVersion();

            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzLabels
                .FirstOrDefaultAsync(x => x.LabelId == id);

            if (entity == null)
                return false;

            _context.ZzLabels.Remove(entity);
            await _context.SaveChangesAsync();

            await IncrementLanguageVersion();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<LabelRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzLabels
                .AsNoTracking()
                .Where(x => x.LabelId == id)
                .Select(x => new LabelRequestDto
                {
                    LableId = x.LabelId,
                    LabelName = x.LabelName,
                    LabelValue = x.LabelValue,
                    LabelNewValue = x.LabelNewValue,
                    Arabic = x.Arabic,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<LabelRequestDto>> GetByFilterAsync(LabelFilterDto filter)
        {
            var query = _context.ZzLabels
                .AsNoTracking()
                .AsQueryable();

            // Filter by LabelName (exact / partial)
            if (!string.IsNullOrWhiteSpace(filter.LabelName))
            {
                query = query.Where(x => x.LabelName.Contains(filter.LabelName));
            }

            // Filter by LabelValue
            if (!string.IsNullOrWhiteSpace(filter.LabelValue))
            {
                query = query.Where(x => x.LabelValue.Contains(filter.LabelValue));
            }

            // Filter by HasNewValue
            if (filter.HasNewValue.HasValue)
            {
                if (filter.HasNewValue.Value)
                    query = query.Where(x => x.LabelNewValue != null && x.LabelNewValue != "");
                else
                    query = query.Where(x => x.LabelNewValue == null || x.LabelNewValue == "");
            }

            // Filter by Created Date range
            if (filter.CreatedFrom.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= filter.CreatedFrom.Value);
            }

            if (filter.CreatedTo.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= filter.CreatedTo.Value);
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.LabelName.ToLower().Contains(search) ||
                    x.LabelValue.ToLower().Contains(search) ||
                    (x.LabelNewValue != null && x.LabelNewValue.ToLower().Contains(search))
                );
            }


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.LabelId)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new LabelRequestDto
                {
                    LableId = x.LabelId,
                    LabelName = x.LabelName,
                    LabelValue = x.LabelValue,
                    LabelNewValue = x.LabelNewValue,
                    Arabic = x.Arabic,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<LabelRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



        public async Task<LanguageLabelsDto> GetTranslations(LanguageCode language)
        {
            var labels = await _context.ZzLabels.ToListAsync();

            var dictionary = labels.ToDictionary(
                x => x.LabelName,
                x => language == LanguageCode.Ar
                    ? (string.IsNullOrEmpty(x.Arabic) ? x.LabelValue : x.Arabic)
                    : x.LabelValue
            );


            return new LanguageLabelsDto
            {
                Language = language.ToString().ToLower(),
                Rtl = IsRtl(language),
                Translations = dictionary
            };
        }


        #region Private Methods


        // the UILanguageVersion must be incremented
        // whenever any label data changes.
        private async Task IncrementLanguageVersion()
        {
            var setting = await _context.ZzGeneralSettings
                .FirstAsync(x => x.ConfigKey == "UILanguageVersion");

            if (!int.TryParse(setting.ConfigValue, out int version))
            {
                version = 0;
            }

            setting.ConfigValue = (version + 1).ToString();

            await _context.SaveChangesAsync();
        }



        public static bool IsRtl(LanguageCode language)
        {
            return language switch
            {
                LanguageCode.Ar => true,
                _ => false
            };
        }



        #endregion


    }
}
