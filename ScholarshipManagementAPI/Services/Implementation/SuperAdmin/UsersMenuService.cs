using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersMenuService : IUsersMenuService
    {
        private readonly AppDbContext _context;

        public UsersMenuService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersMenuRequestdto dto)
        {

            if (await _context.KfUsersMenus
                .AnyAsync(x => x.ActualName.ToLower() == dto.ActualName.ToLower()))
            {
                throw new CustomException($"Page with the ActualName '{dto.ActualName}' already exists.");
            }

            var entity = new KfUsersMenu
            {
                // MenuLinkId = dto.MenuLinkId.Value,
                // MenuLinkId → DO NOT SET

                ModuleId = dto.ModuleId,
                PageHeading = dto.PageHeading,
                ParentId = dto.ParentId,
                PagePath = dto.PagePath,
                ActualName = dto.ActualName.ToLower(),
                IsView = dto.IsView,
                IsActive = true,                  // default to true on creation
                LevelNo = dto.LevelNo,
                SequenceNo = dto.SequenceNo,

                Icon = dto.Icon,

                CreatedBy = dto.CreatedBy,        // or from token
                CreatedDate = dto.CreatedDate     // always server-side
            };

            _context.KfUsersMenus.Add(entity);
            await _context.SaveChangesAsync();

            return entity.MenuLinkId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersMenuRequestdto dto)
        {
            if (dto.MenuLinkId == null || dto.MenuLinkId == 0)
                return false;

            if (await _context.KfUsersMenus.AnyAsync(x =>
                    x.ActualName.ToLower() == dto.ActualName.ToLower()
                    && x.MenuLinkId != dto.MenuLinkId))
            {
                throw new CustomException($"Page with the ActualName '{dto.ActualName}' already exists.");
            }


            var entity = await _context.KfUsersMenus
                .FirstOrDefaultAsync(x => x.MenuLinkId == dto.MenuLinkId);

            if (entity == null)
                return false;

            //MenuLinkId = dto.MenuLinkId.Value,
            //entity.IsActive = dto.IsActive;

            entity.ModuleId = dto.ModuleId;
            entity.PageHeading = dto.PageHeading;
            entity.ParentId = dto.ParentId;
            entity.PagePath = dto.PagePath;

            entity.ActualName = dto.ActualName;

            entity.IsView = dto.IsView;

            entity.LevelNo = dto.LevelNo;
            entity.SequenceNo = dto.SequenceNo;

            entity.Icon = dto.Icon;
            entity.IsActive = true;

            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = dto.UpdatedDate;

            // entity.CreatedBy = dto.CreatedBy;        
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.KfUsersMenus
                .FirstOrDefaultAsync(x => x.MenuLinkId == id);

            if (entity == null)
                return false;

            // Check if this menu has children
            var hasChildren = await _context.KfUsersMenus
                .AnyAsync(x => x.ParentId == id);

            if (hasChildren)
                throw new CustomException("Delete failed: This menu has child items. Please remove or reassign them first.");

            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<UsersMenuRequestdto?> GetByIdAsync(long id)
        {
            return await _context.KfUsersMenus
                .AsNoTracking()
                .Where(x => x.MenuLinkId == id && x.IsActive)
                .Select(x => new UsersMenuRequestdto
                {
                    MenuLinkId = x.MenuLinkId,
                    ModuleId = x.ModuleId,
                    PageHeading = x.PageHeading,
                    ParentId = x.ParentId,
                    PagePath = x.PagePath,
                    ActualName = x.ActualName,
                    IsView = x.IsView,
                    LevelNo = x.LevelNo,
                    SequenceNo = x.SequenceNo,
                    Icon = x.Icon,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation != null ? x.CreatedByNavigation.Staff.StaffFirstName : null,
                    UpdatedBy = x.UpdatedBy??0,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedByName = x.UpdatedByNavigation != null ? x.UpdatedByNavigation.Staff.StaffFirstName : null,
                    ModuleName = x.Module.ModuleName,
                    ParentName = x.Parent != null ? x.Parent.PageHeading : null
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UsersMenuRequestdto>> GetByFilterAsync(UsersMenuFilterDto filter)
        {
            var query = _context.KfUsersMenus
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            // Module filter
            if (filter.ModuleId.HasValue)
                query = query.Where(x => x.ModuleId == filter.ModuleId);

            // Parent menu filter
            if (filter.ParentId.HasValue)
                query = query.Where(x => x.ParentId == filter.ParentId);

            // View permission filter
            if (filter.IsView.HasValue)
                query = query.Where(x => x.IsView == filter.IsView);


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.PageHeading.ToLower().Contains(search) ||
                    x.PagePath.ToLower().Contains(search) ||
                    x.ActualName.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query
                .OrderBy(x => x.ParentId ?? x.MenuLinkId)
                .ThenBy(x => x.LevelNo)
                .ThenBy(x => x.SequenceNo);


            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UsersMenuRequestdto
                {
                    MenuLinkId = x.MenuLinkId,
                    ModuleId = x.ModuleId,
                    PageHeading = x.PageHeading,
                    ParentId = x.ParentId,
                    PagePath = x.PagePath,
                    ActualName = x.ActualName,
                    IsView = x.IsView,
                    LevelNo = x.LevelNo,
                    SequenceNo = x.SequenceNo,
                    Icon = x.Icon,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation != null ? x.CreatedByNavigation.Staff.StaffFirstName : null,
                    UpdatedBy = x.UpdatedBy ?? 0,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedByName = x.UpdatedByNavigation != null ? x.UpdatedByNavigation.Staff.StaffFirstName : null,

                    ModuleName = x.Module.ModuleName,
                    ParentName = x.Parent != null ? x.Parent.PageHeading : null
                })
                .ToListAsync();

            return new PagedResultDto<UsersMenuRequestdto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



    }
}
