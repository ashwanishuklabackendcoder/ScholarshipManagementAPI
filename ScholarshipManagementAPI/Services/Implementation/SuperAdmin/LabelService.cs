using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Label;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Languages;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class LabelService : ILabelService
    {
        private readonly AppDbContext _context;
        private readonly ILocalizationService _localizationService;

        public LabelService(AppDbContext context, ILocalizationService localizationService)
        {
            _context = context;
            _localizationService = localizationService;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(LabelRequestDto dto)
        {
            if (await _context.ZzLabels.AnyAsync(x =>
                x.ModuleId == dto.ModuleId &&
                x.LabelKey.ToLower() == dto.LabelKey.ToLower()))
            {
                throw new CustomException("Label key already exists for this module");
            }

            var entity = new ZzLabel
            {
                ModuleId = dto.ModuleId,
                LabelKey = dto.LabelKey,
                LabelValue = dto.LabelValue,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.ZzLabels.Add(entity);
            await _context.SaveChangesAsync();

            await _localizationService.ClearAllLocalizationCacheAsync();

            return entity.LabelId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(LabelRequestDto dto)
        {
            if (dto.LabelId == null || dto.LabelId == 0)
                return false;

            if (await _context.ZzLabels.AnyAsync(x =>
                x.ModuleId == dto.ModuleId &&
                x.LabelKey.ToLower() == dto.LabelKey.ToLower() &&
                x.LabelId != dto.LabelId))
            {
                throw new CustomException("Label key already exists for this module");
            }

            var entity = await _context.ZzLabels
                .FirstOrDefaultAsync(x => x.LabelId == dto.LabelId);

            if (entity == null)
                return false;

            // ModuleId is not updated on purpose, as it may break the existing translations.
            //entity.ModuleId = dto.ModuleId;

            entity.LabelKey = dto.LabelKey;
            entity.LabelValue = dto.LabelValue;
            entity.IsActive = true;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();

            await _localizationService.ClearAllLocalizationCacheAsync();

            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzLabels
                .FirstOrDefaultAsync(x => x.LabelId == id);

            if (entity == null)
                return false;

            //_context.ZzLabels.Remove(entity);
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            await _localizationService.ClearAllLocalizationCacheAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<LabelRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzLabels
                .AsNoTracking()
                .Where(x => x.LabelId == id && x.IsActive)
                .Select(x => new LabelRequestDto
                {
                    LabelId = x.LabelId,
                    LabelKey = x.LabelKey,
                    LabelValue = x.LabelValue,

                    ModuleId = x.ModuleId,
                    ModuleName = x.Module != null ? x.Module.ModuleName : null,

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
        public async Task<PagedResultDto<LabelRequestDto>> GetByFilterAsync(LabelFilterDto filter)
        {
            var query = _context.ZzLabels
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (filter.ModuleId.HasValue)
            {
                query = query.Where(x => x.ModuleId == filter.ModuleId.Value);
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.LabelKey.ToLower().Contains(search) ||
                    x.LabelValue.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query
                .OrderBy(x => x.ModuleId)
                .ThenBy(x => x.LabelKey);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new LabelRequestDto
                {
                    LabelId = x.LabelId,
                    LabelKey = x.LabelKey,
                    LabelValue = x.LabelValue,

                    ModuleId = x.ModuleId,
                    ModuleName = x.Module != null ? x.Module.ModuleName : null,

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

            return new PagedResultDto<LabelRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




    }
}
