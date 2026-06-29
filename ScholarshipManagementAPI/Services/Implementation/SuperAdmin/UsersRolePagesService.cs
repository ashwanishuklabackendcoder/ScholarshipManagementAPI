using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersRolePagesService: IUsersRolePagesService
    {
        private readonly AppDbContext _context;

        public UsersRolePagesService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersRolePageRequestDto dto)
        {
            if (await _context.UsersRolePages
                .AnyAsync(x => x.RoleId == dto.RoleId && x.MenuLinkId == dto.MenuLinkId))
            {
                throw new CustomException("Role with same MenuLink already exists");
            }


            var entity = new UsersRolePage
            {
                RoleId = dto.RoleId,
                MenuLinkId = dto.MenuLinkId,
                InsertPer = dto.InsertPer,
                UpdatePer = dto.UpdatePer,
                DeletePer = dto.DeletePer,
                ViewPer = dto.ViewPer,

                CreatedDate = DateTime.UtcNow     // always server-side
            };

            _context.UsersRolePages.Add(entity);
            await _context.SaveChangesAsync();

            return entity.RoleId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersRolePageRequestDto dto)
        {
            if (dto.RoleFormId == null || dto.RoleId == 0)
                return false;

            if (await _context.UsersRolePages.AnyAsync(x =>
                      x.RoleId == dto.RoleId && x.MenuLinkId == dto.MenuLinkId
                      && x.RoleId != dto.RoleId))
            {
                throw new CustomException("Role with same MenuLink already exists");
            }

            var entity = await _context.UsersRolePages
                .FirstOrDefaultAsync(x => x.RoleFormId == dto.RoleFormId);

            if (entity == null)
                return false;


            entity.RoleId = dto.RoleId;
            entity.MenuLinkId = dto.MenuLinkId;
            entity.InsertPer = dto.InsertPer;
            entity.UpdatePer = dto.UpdatePer;
            entity.DeletePer = dto.DeletePer;
            entity.ViewPer = dto.ViewPer;

            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UsersRolePages
                .FirstOrDefaultAsync(x => x.RoleFormId == id);

            if (entity == null)
                return false;

            _context.UsersRolePages.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<UsersRolePageRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UsersRolePages
                .AsNoTracking()
                .Where(x => x.RoleFormId == id)
                .Select(x => new UsersRolePageRequestDto
                {
                    RoleFormId = x.RoleFormId,
                    RoleId = x.RoleId,
                    MenuLinkId = x.MenuLinkId,
                    InsertPer = x.InsertPer,
                    UpdatePer = x.UpdatePer,
                    DeletePer = x.DeletePer,
                    ViewPer = x.ViewPer,
                    CreatedDate = x.CreatedDate,

                    Module = x.Role.Module.ModuleName,
                    RoleName = x.Role.RoleName,
                    PageHeading = x.MenuLink != null ? x.MenuLink.PageHeading : null,
                    PagePath = x.MenuLink != null ? x.MenuLink.PagePath : null,
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UsersRolePageRequestDto>> GetByFilterAsync(UsersRolePageFilterDto filter)
        {
            var query = _context.UsersRolePages
                .AsNoTracking()
                .Include(x => x.Role)
                .Include(x => x.MenuLink)
                .AsQueryable();

            /* -------- RolePage filters -------- */
            if (filter.RoleId.HasValue)
                query = query.Where(x => x.RoleId == filter.RoleId);

            if (filter.MenuLinkId.HasValue)
                query = query.Where(x => x.MenuLinkId == filter.MenuLinkId);


            /* -------- Role based filters -------- */
            if (filter.ModuleId.HasValue)
                query = query.Where(x => x.Role.ModuleId == filter.ModuleId);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.Role.IsActive == filter.IsActive);

            if (filter.DashboardMenuLinkId.HasValue)
                query = query.Where(x => x.Role.DashboardMenuLinkId == filter.DashboardMenuLinkId);



            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.Role.RoleName.ToLower().Contains(search) ||
                    (x.Role.Description != null && x.Role.Description.ToLower().Contains(search)) ||
                    (x.MenuLink != null && x.MenuLink.PageHeading.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.RoleId).ThenBy(x => x.MenuLinkId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UsersRolePageRequestDto
                {
                    RoleFormId = x.RoleFormId,
                    RoleId = x.RoleId,
                    MenuLinkId = x.MenuLinkId,
                    InsertPer = x.InsertPer,
                    UpdatePer = x.UpdatePer,
                    DeletePer = x.DeletePer,
                    ViewPer = x.ViewPer,
                    CreatedDate = x.CreatedDate,

                    Module = x.Role.Module.ModuleName,
                    RoleName = x.Role.RoleName,
                    PageHeading = x.MenuLink != null ? x.MenuLink.PageHeading : null,
                    PagePath = x.MenuLink != null ? x.MenuLink.PagePath : null,
                })
                .ToListAsync();

            return new PagedResultDto<UsersRolePageRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task<PagedResultDto<RolePagePermissionDto>> GetRolePermissionsAsync(UsersRolePageFilterDto filter)
        {
            var menus = await _context.UsersMenus
                .AsNoTracking()
                .Include(x => x.Module)
                .OrderBy(x => x.PageHeading)
                .ToListAsync();


            var permissions = await _context.UsersRolePages
                .Where(x => x.RoleId == filter.RoleId)
                .Include(x => x.Role)
                .ToListAsync();

            var query = menus.Select(menu =>
            {
                var p = permissions.FirstOrDefault(x => x.MenuLinkId == menu.MenuLinkId);

                return new RolePagePermissionDto
                {
                    MenuLinkId = menu.MenuLinkId,
                    RoleId = filter.RoleId ?? 0,

                    RoleFormId = p?.RoleFormId,

                    ViewPer = p?.ViewPer ?? false,
                    InsertPer = p?.InsertPer ?? false,
                    UpdatePer = p?.UpdatePer ?? false,
                    DeletePer = p?.DeletePer ?? false,

                    RoleName = p?.Role?.RoleName,
                    Module = menu.Module.ModuleName,
                    PageHeading = menu.PageHeading,
                    PagePath = menu.PagePath,

                    ParentId = menu.ParentId,
                    LevelNo = menu.LevelNo,
                    SequenceNo = menu.SequenceNo

                };
            }).AsQueryable();


            // ---------- Total Count ----------
            var totalCount = query.Count();

            // ---------- Ordering ----------
            query = query
               .OrderBy(x => x.ParentId ?? x.MenuLinkId)
               .ThenBy(x => x.LevelNo)
               .ThenBy(x => x.SequenceNo);

            // ---------- Pagination ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }


            var items = query.ToList();

            return new PagedResultDto<RolePagePermissionDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task BulkSaveRolePermissionsAsync(RolePermissionBulkSaveDto dto, string createdBy)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var existing = await _context.UsersRolePages
                    .Where(x => x.RoleId == dto.RoleId)
                    .ToListAsync();

                var existingDict = existing.ToDictionary(x => x.MenuLinkId);

                foreach (var item in dto.Permissions)
                {
                    var hasPermission =
                        item.ViewPer || item.InsertPer || item.UpdatePer || item.DeletePer;

                    existingDict.TryGetValue(item.MenuLinkId, out var existingRecord);

                    // INSERT
                    if (hasPermission && existingRecord == null)
                    {
                        _context.UsersRolePages.Add(new UsersRolePage
                        {
                            RoleId = dto.RoleId,
                            MenuLinkId = item.MenuLinkId,
                            ViewPer = item.ViewPer,
                            InsertPer = item.InsertPer,
                            UpdatePer = item.UpdatePer,
                            DeletePer = item.DeletePer,
                            CreatedDate = DateTime.UtcNow,
                            //CreatedBy = createdBy
                        });
                    }

                    // UPDATE
                    else if (hasPermission && existingRecord != null)
                    {
                        existingRecord.ViewPer = item.ViewPer;
                        existingRecord.InsertPer = item.InsertPer;
                        existingRecord.UpdatePer = item.UpdatePer;
                        existingRecord.DeletePer = item.DeletePer;
                    }

                    // DELETE
                    else if (!hasPermission && existingRecord != null)
                    {
                        _context.UsersRolePages.Remove(existingRecord);
                    }
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

    }
}
