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
            var exists = await _context.ZzMasterDropdowns
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
                uniqueId = await _context.ZzMasterDropdowns
                    .Where(x => x.ParentId == null)
                    .MaxAsync(x => (long?)x.UniqueId) ?? 0;

                uniqueId++;

                if (uniqueId > 300)
                    throw new CustomException("Parent dropdown limit exceeded (1-300 reserved)");

                displaySequence = (int)uniqueId;
            }
            else // Child dropdown value
            {
                uniqueId = await _context.ZzMasterDropdowns
                    .Where(x => x.UniqueId >= 301)
                    .MaxAsync(x => (long?)x.UniqueId) ?? 300;

                uniqueId++;

                displaySequence = await _context.ZzMasterDropdowns
                    .Where(x => x.ParentId == dto.ParentId)
                    .MaxAsync(x => (int?)x.DisplaySequence) ?? 0;

                displaySequence++;
            }

            var entity = new ZzMasterDropdown
            {
                UniqueId = uniqueId,
                DisplayText = dto.DisplayText,
                ParentId = dto.ParentId == null || dto.ParentId == 0 ? null : dto.ParentId,
                DisplaySequence = displaySequence,
                IsActive = true,

                CreatedBy = dto.CreatedBy,        // or from token
                CreatedDate = DateTime.UtcNow     // always server-side
            };

            _context.ZzMasterDropdowns.Add(entity);
            await _context.SaveChangesAsync();

            return entity.UniqueId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(MasterDropDownRequestDto dto)
        {
            if (dto.UniqueId == null || dto.UniqueId == 0)
                return false;

            var exists = await _context.ZzMasterDropdowns
                .AnyAsync(x =>
                    x.ParentId == (dto.ParentId == 0 ? null : dto.ParentId) &&
                    x.DisplayText.ToLower().Trim() == dto.DisplayText.ToLower().Trim() &&
                    x.UniqueId != dto.UniqueId);

            if (exists)
            {
                throw new CustomException("Dropdown with same display text already exists");
            }

            var entity = await _context.ZzMasterDropdowns
                .FirstOrDefaultAsync(x => x.UniqueId == dto.UniqueId);

            if (entity == null)
                return false;

            if (entity.ParentId == null)
                throw new CustomException("System dropdown cannot be modified");

            // not changed
            // entity.UniqueId = dto.UniqueId; 
            entity.DisplayText = dto.DisplayText;

            if (entity.ParentId != null)
                entity.DisplaySequence = dto.DisplaySequence;

            entity.IsActive = true;
            entity.UpdatedBy = dto.UpdatedBy;        // or from token
            entity.UpdatedDate = DateTime.UtcNow;    // always server-side

            // CreatedDate NOT updated on purpose
            // CreatedBy NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzMasterDropdowns
                .FirstOrDefaultAsync(x => x.UniqueId == id);

            if (entity == null)
                return false;

            if (entity.ParentId == null)
                throw new CustomException("System dropdown cannot be deleted");

            entity.IsActive = false;
            //_context.ZzMasterDropDowns.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<MasterDropDownRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzMasterDropdowns
                .AsNoTracking()
                .Where(x => x.UniqueId == id && x.IsActive)
                .Select(x => new MasterDropDownRequestDto
                {
                    UniqueId = x.UniqueId,
                    DisplayText = x.DisplayText,
                    ParentId = x.ParentId,
                    ParentName = x.Parent != null ? x.Parent.DisplayText : null,
                    DisplaySequence = x.DisplaySequence,
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
        public async Task<PagedResultDto<MasterDropDownRequestDto>> GetByFilterAsync(MasterDropDownFilterDto filter)
        {
            var query = _context.ZzMasterDropdowns
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable();

            if (filter.ParentId.HasValue)
                query = query.Where(x => x.ParentId == filter.ParentId);
            else
                query = query.Where(x => x.ParentId == null);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.DisplayText.ToLower().Contains(search)
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
                    ParentName = x.Parent != null ? x.Parent.DisplayText : null,
                    DisplaySequence = x.DisplaySequence,
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

            return new PagedResultDto<MasterDropDownRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task<List<MasterDropDownRequestDto>> GetByParentIdAsync(long parentId)
        {
            return await _context.ZzMasterDropdowns
                .AsNoTracking()
                .Where(x => x.ParentId == parentId && x.IsActive)
                .OrderBy(x => x.DisplaySequence)
                .Select(x => new MasterDropDownRequestDto
                {
                    UniqueId = x.UniqueId,
                    DisplayText = x.DisplayText,
                    ParentId = x.ParentId,
                    ParentName = x.Parent != null ? x.Parent.DisplayText : null,
                    DisplaySequence = x.DisplaySequence,
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
        }


    }

}
