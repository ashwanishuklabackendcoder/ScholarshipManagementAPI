using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;


namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class MasterDropDownService : IMasterDropDownService
    {
        private readonly AppDbContext _context;

        public MasterDropDownService(AppDbContext context)
        {
            _context = context;
        }

        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(MasterDropDownRequestDto dto)
        {
            var exists = await _context.ZzMasterDropDowns
                .AnyAsync(x =>
                    x.ParentId == (dto.ParentId == 0 ? null : dto.ParentId) &&
                    x.DisplayText.ToLower() == dto.DisplayText.ToLower());

            if (exists)
            {
                throw new CustomException("Dropdown with same display text already exists");
            }

            long uniqueId;
            int displaySequence;

            if (dto.ParentId == null || dto.ParentId == 0) // Parent dropdown
            {
                uniqueId = await _context.ZzMasterDropDowns
                    .Where(x => x.ParentId == null)
                    .MaxAsync(x => (long?)x.UniqueId) ?? 0;

                uniqueId++;

                if (uniqueId > 300)
                    throw new CustomException("Parent dropdown limit exceeded (1-300 reserved)");

                displaySequence = (int)uniqueId;
            }
            else // Child dropdown value
            {
                uniqueId = await _context.ZzMasterDropDowns
                    .Where(x => x.UniqueId >= 301)
                    .MaxAsync(x => (long?)x.UniqueId) ?? 300;

                uniqueId++;

                displaySequence = await _context.ZzMasterDropDowns
                    .Where(x => x.ParentId == dto.ParentId)
                    .MaxAsync(x => (int?)x.DisplaySequence) ?? 0;

                displaySequence++;
            }

            var entity = new ZzMasterDropDown
            {
                UniqueId = uniqueId,
                DisplayText = dto.DisplayText,
                ParentId = dto.ParentId == null || dto.ParentId == 0 ? null : dto.ParentId,
                DisplaySequence = displaySequence,
                Status = dto.Status,
                IsEditable = dto.ParentId == null || dto.ParentId == 0 ? false : true,
                IsShow = dto.IsShow,
                ModuleId = dto.ParentId == null || dto.ParentId == 0 ? dto.ModuleId : null,

                CreatedBy = dto.CreatedBy,        // or from token
                CreatedDate = DateTime.UtcNow     // always server-side
            };

            _context.ZzMasterDropDowns.Add(entity);
            await _context.SaveChangesAsync();

            return entity.UniqueId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(MasterDropDownRequestDto dto)
        {
            if (dto.UniqueId == null || dto.UniqueId == 0)
                return false;

            var exists = await _context.ZzMasterDropDowns
                .AnyAsync(x =>
                    x.ParentId == (dto.ParentId == 0 ? null : dto.ParentId) &&
                    x.DisplayText.ToLower().Trim() == dto.DisplayText.ToLower().Trim() &&
                    x.UniqueId != dto.UniqueId);

            if (exists)
            {
                throw new CustomException("Dropdown with same display text already exists");
            }

            var entity = await _context.ZzMasterDropDowns
                .FirstOrDefaultAsync(x => x.UniqueId == dto.UniqueId);

            if (entity == null)
                return false;

            if (entity.ParentId == null && !entity.IsEditable)
                throw new CustomException("System dropdown cannot be modified");

            // not changed
            // entity.UniqueId = dto.UniqueId; 
            entity.DisplayText = dto.DisplayText;

            if (entity.ParentId != null)
                entity.ParentId = dto.ParentId;

            if (entity.ParentId != null)
                entity.DisplaySequence = dto.DisplaySequence;

            entity.Status = dto.Status;
            entity.IsEditable = dto.IsEditable;
            entity.IsShow = dto.IsShow;
            entity.ModuleId = dto.ModuleId == 0 ? null : dto.ModuleId;

            // CreatedDate NOT updated on purpose
            // CreatedBy NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzMasterDropDowns
                .FirstOrDefaultAsync(x => x.UniqueId == id);

            if (entity == null)
                return false;

            if (entity.ParentId == null && !entity.IsEditable)
                throw new CustomException("System dropdown cannot be deleted");

            _context.ZzMasterDropDowns.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<MasterDropDownRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzMasterDropDowns
                .AsNoTracking()
                .Include(x => x.Module)
                .Where(x => x.UniqueId == id)
                .Select(x => new MasterDropDownRequestDto
                {
                    UniqueId = x.UniqueId,
                    DisplayText = x.DisplayText,
                    ParentId = x.ParentId,
                    DisplaySequence = x.DisplaySequence,
                    Status = x.Status,
                    IsEditable = x.IsEditable,
                    IsShow = x.IsShow,
                    CreatedBy = x.CreatedBy,
                    ModuleId = x.ModuleId,
                    CreatedDate = x.CreatedDate,
                    ModuleName = x.Module != null ? x.Module.ModuleName : null
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<MasterDropDownRequestDto>> GetByFilterAsync(MasterDropDownFilterDto filter)
        {
            var query = _context.ZzMasterDropDowns
                .AsNoTracking()
                .Include(x => x.Module)
                .AsQueryable();

            if (filter.ModuleId.HasValue)
                query = query.Where(x => x.ModuleId == filter.ModuleId);

            if (filter.ParentId.HasValue)
                query = query.Where(x => x.ParentId == filter.ParentId);
            else
                query = query.Where(x => x.ParentId == null);

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status);

            if (filter.IsShow.HasValue)
                query = query.Where(x => x.IsShow == filter.IsShow);


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.DisplayText.ToLower().Contains(search) ||
                    (x.CreatedBy != null && x.CreatedBy.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.DisplaySequence);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new MasterDropDownRequestDto
                {
                    UniqueId = x.UniqueId,
                    DisplayText = x.DisplayText,
                    ParentId = x.ParentId,
                    DisplaySequence = x.DisplaySequence,
                    Status = x.Status,
                    IsEditable = x.IsEditable,
                    IsShow = x.IsShow,
                    CreatedBy = x.CreatedBy,
                    ModuleId = x.ModuleId,
                    CreatedDate = x.CreatedDate,
                    ModuleName = x.Module != null ? 
                                 x.Module.ModuleName : 
                                 (x.Parent != null && x.Parent.Module != null ? x.Parent.Module.ModuleName : null)
                })
                .ToListAsync();

            return new PagedResultDto<MasterDropDownRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }

}
