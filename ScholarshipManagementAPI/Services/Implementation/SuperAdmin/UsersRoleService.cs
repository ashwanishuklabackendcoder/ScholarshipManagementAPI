using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersRoleService : IUsersRoleService
    {
        private readonly AppDbContext _context;

        public UsersRoleService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersRoleRequestDto dto)
        {
            if (await _context.UsersRoles
                .AnyAsync(x => x.RoleName.ToLower() == dto.RoleName.ToLower()))
            {
                throw new CustomException("Role with same RoleName already exists");
            }


            var entity = new UsersRole
            {
                RoleName = dto.RoleName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                ModuleId = dto.ModuleId,
                DashboardMenuLinkId = dto.DashboardMenuLinkId,


                CreatedBy = dto.CreatedBy != null ? dto.CreatedBy : "",        // or from token
                CreatedDate = dto.CreatedDate                                  // always server-side
            };

            _context.UsersRoles.Add(entity);
            await _context.SaveChangesAsync();

            return entity.RoleId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersRoleRequestDto dto)
        {
            if (dto.RoleId == null || dto.RoleId == 0)
                return false;

            if (await _context.UsersRoles.AnyAsync(x =>
                      x.RoleName.ToLower() == dto.RoleName.ToLower()
                      && x.RoleId != dto.RoleId))
            {
                throw new CustomException("Role with same RoleName already exists");
            }

            var entity = await _context.UsersRoles
                .FirstOrDefaultAsync(x => x.RoleId == dto.RoleId);

            if (entity == null)
                return false;

            
            entity.RoleName = dto.RoleName;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.ModuleId = dto.ModuleId;
            entity.DashboardMenuLinkId = dto.DashboardMenuLinkId;

            // entity.CreatedBy = dto.CreatedBy;        
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UsersRoles
                .FirstOrDefaultAsync(x => x.RoleId == id);

            if (entity == null)
                return false;

            _context.UsersRoles.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<UsersRoleRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UsersRoles
                .AsNoTracking()
                .Where(x => x.RoleId == id)
                .Select(x => new UsersRoleRequestDto
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Description = x.Description,
                    ModuleId = x.ModuleId,
                    DashboardMenuLinkId = x.DashboardMenuLinkId,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    ModuleName = x.Module.ModuleName,
                    DashboardMenuName = x.DashboardMenuLink != null ? x.DashboardMenuLink.PageHeading : null,
                    DashboardPath = x.DashboardMenuLink != null ? x.DashboardMenuLink.PagePath : null
                
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UsersRoleRequestDto>> GetByFilterAsync(UsersRoleFilterDto filter)
        {
            var query = _context.UsersRoles
                .AsNoTracking()
                .Include(x => x.Module)
                .Include(x => x.DashboardMenuLink)
                .AsQueryable();

            // Module filter
            if (filter.ModuleId.HasValue)
                query = query.Where(x => x.ModuleId == filter.ModuleId);

            // DashboardMenuLink filter
            if (filter.DashboardMenuLinkId.HasValue)
                query = query.Where(x => x.DashboardMenuLinkId == filter.DashboardMenuLinkId);

            // IsActive filter
            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);



            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.RoleName.ToLower().Contains(search) ||
                    (x.Description!=null && x.Description.ToLower().Contains(search)) ||
                    (x.DashboardMenuLink != null && x.DashboardMenuLink.PageHeading.ToLower().Contains(search)) ||
                    (x.Module != null && x.Module.ModuleName.ToLower().Contains(search)) 
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.ModuleId).ThenBy(x => x.RoleId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UsersRoleRequestDto
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Description = x.Description,
                    ModuleId = x.ModuleId,
                    DashboardMenuLinkId = x.DashboardMenuLinkId,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    ModuleName = x.Module.ModuleName,
                    DashboardMenuName = x.DashboardMenuLink != null ? x.DashboardMenuLink.PageHeading : null,
                    DashboardPath = x.DashboardMenuLink != null ? x.DashboardMenuLink.PagePath : null
               
                })
                .ToListAsync();

            return new PagedResultDto<UsersRoleRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
