using Amazon.S3.Model;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLogin;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersLoginRoleService : IUsersLoginRoleService
    {
        private readonly AppDbContext _context;

        public UsersLoginRoleService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersLoginRoleRequestDto dto)
        {
            if (await _context.UsersLoginRoles
                .AnyAsync(x => x.RoleId == dto.RoleId && x.LoginId == dto.LoginId))
            {
                throw new CustomException("Role with user already exists");
            }

            // Default role check
            if (dto.IsDefault)
            {
                var alreadyDefault = await _context.UsersLoginRoles
                    .AnyAsync(x => x.LoginId == dto.LoginId && x.IsDefault);

                if (alreadyDefault)
                {
                    throw new CustomException("Default role already exists for this user");
                }
            }


            var entity = new UsersLoginRole
            {
                RoleId = dto.RoleId,
                LoginId = dto.LoginId,
                IsDefault = dto.IsDefault,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate     // always server-side
            };

            _context.UsersLoginRoles.Add(entity);
            await _context.SaveChangesAsync();

            return entity.LoginId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersLoginRoleRequestDto dto)
        {
            if (dto.UserLoginRoleId == null || dto.UserLoginRoleId == 0)
                return false;

            if (await _context.UsersLoginRoles.AnyAsync(x =>
                      x.RoleId == dto.RoleId && x.LoginId == dto.LoginId
                      && x.UserLoginRoleId != dto.UserLoginRoleId))
            {
                throw new CustomException("Role with user already exists");
            }

            var entity = await _context.UsersLoginRoles
                .FirstOrDefaultAsync(x => x.UserLoginRoleId == dto.UserLoginRoleId);

            if (entity == null)
                return false;


            if (dto.IsDefault)
            {
                var alreadyDefault = await _context.UsersLoginRoles
                    .AnyAsync(x =>
                        x.LoginId == dto.LoginId &&
                        x.IsDefault &&
                        x.UserLoginRoleId != dto.UserLoginRoleId);   // ⭐ ignore same record

                if (alreadyDefault)
                {
                    throw new CustomException("Default role already exists for this user");
                }
            }


            entity.RoleId = dto.RoleId;
            entity.LoginId = dto.LoginId;
            entity.IsDefault = dto.IsDefault;

            // CREATEDBY and CREATEDDATE not updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UsersLoginRoles
                .FirstOrDefaultAsync(x => x.UserLoginRoleId == id);

            if (entity == null)
                return false;

            _context.UsersLoginRoles.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<UsersLoginRoleRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UsersLoginRoles
                .AsNoTracking()
                .Include(x => x.Login)
                .Include(x => x.Role)
                .Where(x => x.UserLoginRoleId == id)
                .Select(x => new UsersLoginRoleRequestDto
                {
                    UserLoginRoleId = x.UserLoginRoleId,
                    RoleId = x.RoleId,
                    LoginId = x.LoginId,
                    IsDefault = x.IsDefault,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,

                    LoginName = x.Login.LoginName,
                    RoleName = x.Role.RoleName,
                    Module = x.Role.Module.ModuleName
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UsersLoginRoleRequestDto>> GetByFilterAsync(UsersLoginRoleFilterDto filter)
        {
            var query = _context.UsersLoginRoles
                .AsNoTracking()
                .Include(x => x.Login)
                .Include(x => x.Role)
                .AsQueryable();


            if (filter.RoleId.HasValue)
                query = query.Where(x => x.RoleId == filter.RoleId);

            if (filter.LoginId.HasValue)
                query = query.Where(x => x.LoginId == filter.LoginId);

            if (filter.IsDefault.HasValue)
                query = query.Where(x => x.IsDefault == filter.IsDefault);

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.Role.RoleName.ToLower().Contains(search) ||
                    x.Login.LoginName.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.RoleId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UsersLoginRoleRequestDto
                {
                    UserLoginRoleId = x.UserLoginRoleId,
                    RoleId = x.RoleId,
                    LoginId = x.LoginId,
                    IsDefault = x.IsDefault,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,

                    LoginName = x.Login.LoginName,
                    RoleName = x.Role.RoleName,
                    Module = x.Role.Module.ModuleName
                })
                .ToListAsync();

            return new PagedResultDto<UsersLoginRoleRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task<PagedResultDto<LoginRoleAssignmentDto>> GetRolesByLoginAsync(UsersLoginRoleFilterDto filter)
        {
            var roles = await _context.UsersRoles
                .AsNoTracking()
                .Include(x => x.Module)
                .OrderBy(x => x.RoleName)
                .ToListAsync();

            var mappedRoles = await _context.UsersLoginRoles
                .Where(x => x.LoginId == filter.LoginId)
                .ToListAsync();

            var query = roles.Select(role =>
            {
                var mapped = mappedRoles.FirstOrDefault(x => x.RoleId == role.RoleId);

                return new LoginRoleAssignmentDto
                {
                    RoleId = role.RoleId,
                    LoginId = filter.LoginId ?? 0,

                    UserLoginRoleId = mapped?.UserLoginRoleId,

                    IsMapped = mapped != null,
                    IsDefault = mapped?.IsDefault ?? false,

                    RoleName = role.RoleName,
                    Module = role.Module.ModuleName
                };
            }).AsQueryable();


            /* ---------- Total Count ---------- */
            var totalCount = query.Count();

            /* ---------- Ordering ---------- */
            query = query.OrderBy(x => x.RoleId);


            /* ---------- Pagination ---------- */
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }


            var items = query.ToList();


            return new PagedResultDto<LoginRoleAssignmentDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task BulkSaveRolesAsync(LoginRoleBulkSaveDto dto, string createdBy)
        {
            // multiple role(roleId) assignment for a single user(loginId) is allowed
            // but only one default role is allowed for a single user
            // a role must be mapped before it can be default

            if (dto.Roles.Count(x => x.IsDefault) > 1)
                throw new Exception("Only one default role is allowed for a user.");

            if (dto.Roles.Any(x => x.IsDefault && !x.IsMapped))
                throw new Exception("A role must be mapped before it can be default.");

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var existing = await _context.UsersLoginRoles
                    .Where(x => x.LoginId == dto.LoginId)
                    .ToListAsync();

                var existingDict = existing.ToDictionary(x => x.RoleId);

                foreach (var role in dto.Roles)
                {
                    existingDict.TryGetValue(role.RoleId, out var existingRole);

                    if (role.IsMapped)
                    {
                        if (existingRole == null)
                        {
                            _context.UsersLoginRoles.Add(new UsersLoginRole
                            {
                                LoginId = dto.LoginId,
                                RoleId = role.RoleId,
                                IsDefault = role.IsDefault,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = createdBy
                            });
                        }
                        else
                        {
                            existingRole.IsDefault = role.IsDefault;
                        }
                    }
                    else
                    {
                        if (existingRole != null)
                            _context.UsersLoginRoles.Remove(existingRole);
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
