using Amazon.S3.Model;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRoleAssignment;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersRoleAssignmentService : IUsersRoleAssignmentService
    {
        private readonly AppDbContext _context;

        public UsersRoleAssignmentService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersRoleAssignmentRequestDto dto)
        {
            if (await _context.KfUsersLoginRolesAssignments
                .AnyAsync(x => x.RoleId == dto.RoleId && x.LoginId == dto.LoginId))
            {
                throw new CustomException("Role with user already exists");
            }

            // Default role check
            if (dto.IsDefault)
            {
                var alreadyDefault = await _context.KfUsersLoginRolesAssignments
                    .AnyAsync(x => x.LoginId == dto.LoginId && x.IsDefault);

                if (alreadyDefault)
                {
                    throw new CustomException("Default role already exists for this user");
                }
            }


            var entity = new KfUsersLoginRolesAssignment
            {
                RoleId = dto.RoleId,
                LoginId = dto.LoginId,
                IsDefault = dto.IsDefault,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate     // always server-side
            };

            _context.KfUsersLoginRolesAssignments.Add(entity);
            await _context.SaveChangesAsync();

            return entity.LoginId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersRoleAssignmentRequestDto dto)
        {
            if (dto.UserLoginRoleId == null || dto.UserLoginRoleId == 0)
                return false;

            if (await _context.KfUsersLoginRolesAssignments.AnyAsync(x =>
                      x.RoleId == dto.RoleId && x.LoginId == dto.LoginId
                      && x.UserLoginRoleId != dto.UserLoginRoleId))
            {
                throw new CustomException("Role with user already exists");
            }

            var entity = await _context.KfUsersLoginRolesAssignments
                .FirstOrDefaultAsync(x => x.UserLoginRoleId == dto.UserLoginRoleId);

            if (entity == null)
                return false;


            if (dto.IsDefault)
            {
                var alreadyDefault = await _context.KfUsersLoginRolesAssignments
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
            var entity = await _context.KfUsersLoginRolesAssignments
                .FirstOrDefaultAsync(x => x.UserLoginRoleId == id);

            if (entity == null)
                return false;

            _context.KfUsersLoginRolesAssignments.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<UsersRoleAssignmentRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfUsersLoginRolesAssignments
                .AsNoTracking()
                .Include(x => x.Login)
                .Include(x => x.Role)
                .Where(x => x.UserLoginRoleId == id)
                .Select(x => new UsersRoleAssignmentRequestDto
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
        public async Task<PagedResultDto<UsersRoleAssignmentRequestDto>> GetByFilterAsync(UsersRoleAssignmentFilterDto filter)
        {
            var query = _context.KfUsersLoginRolesAssignments
                .AsNoTracking()
                .Include(x => x.Login)
                .Include(x => x.Role)
                .ThenInclude(r => r.Module)
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
                .Select(x => new UsersRoleAssignmentRequestDto
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

            return new PagedResultDto<UsersRoleAssignmentRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



        public async Task<PagedResultDto<UsersRoleAssignmentDto>> GetRolesByLoginAsync(UsersRoleAssignmentFilterDto filter)
        {
            var roles = await _context.KfUsersRoles
                .AsNoTracking()
                .Include(x => x.Module)
                .OrderBy(x => x.RoleName)
                .ToListAsync();

            var mappedRoles = await _context.KfUsersLoginRolesAssignments
                .Where(x => x.LoginId == filter.LoginId)
                .ToListAsync();

            var query = roles.Select(role =>
            {
                var mapped = mappedRoles.FirstOrDefault(x => x.RoleId == role.RoleId);

                return new UsersRoleAssignmentDto
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


            return new PagedResultDto<UsersRoleAssignmentDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task BulkSaveRolesAsync(UsersRoleAssignmentSaveDto dto, long createdBy)
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
                var existing = await _context.KfUsersLoginRolesAssignments
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
                            _context.KfUsersLoginRolesAssignments.Add(new KfUsersLoginRolesAssignment
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
                            _context.KfUsersLoginRolesAssignments.Remove(existingRole);
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
