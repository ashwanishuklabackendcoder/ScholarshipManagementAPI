using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLogin;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersLoginService : IUsersLoginService
    {
        private readonly AppDbContext _context;

        public UsersLoginService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersLoginRequestDto dto)
        {
            if (await _context.UsersLogins
                .AnyAsync(x => x.LoginName == dto.LoginName))
            {
                throw new CustomException("User with same login name already exists");
            }

            var entity = new UsersLogin
            {
                StaffId = dto.StaffId,

                LoginName = dto.LoginName,
                Password = dto.Password,
                ForgotEmail = dto.ForgotEmail,

                IsActive = dto.IsActive,
                Language = dto.Language,

                //nullable fields - can be set later
                //TempPassDateTime = dto.TempPassDateTime,
                //TempPassword = dto.TempPassword,

                CreatedBy = dto.CreatedBy,        // on controller side - jwt claim
                CreatedDate = DateTime.UtcNow     // always server-side
            };
      
            _context.UsersLogins.Add(entity);
            await _context.SaveChangesAsync();

            return entity.LoginId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersLoginRequestDto dto)
        {
            if (dto.LoginId == null || dto.LoginId == 0)
                return false;

            if (await _context.UsersLogins.AnyAsync(x =>
                      x.LoginName == dto.LoginName
                      && x.LoginId != dto.LoginId))
            {
                throw new CustomException("User with same login name already exists");
            }

            var entity = await _context.UsersLogins
                .FirstOrDefaultAsync(x => x.LoginId == dto.LoginId);

            if (entity == null)
                return false;


            entity.StaffId = dto.StaffId;
            entity.LoginName = dto.LoginName;
            entity.Password = dto.Password;
            entity.ForgotEmail = dto.ForgotEmail;
            entity.IsActive = dto.IsActive;
            entity.TempPassDateTime = dto.TempPassDateTime;
            entity.TempPassword = dto.TempPassword;
            entity.Language = dto.Language;

            // CREATEDBY and CREATEDDATE not updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UsersLogins
                .FirstOrDefaultAsync(x => x.LoginId == id);

            if (entity == null)
                return false;

            _context.UsersLogins.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<UsersLoginRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UsersLogins
                .AsNoTracking()
                .Where(x => x.LoginId == id)
                .Include(x => x.Staff)
                .Select(x => new UsersLoginRequestDto
                {
                    LoginId = x.LoginId,
                    StaffId = x.StaffId,
                    LoginName = x.LoginName,
                    Password = x.Password,
                    ForgotEmail = x.ForgotEmail,
                    IsActive = x.IsActive,
                    TempPassword = x.TempPassword,
                    TempPassDateTime = x.TempPassDateTime,
                    Language = x.Language,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UsersLoginRequestDto>> GetByFilterAsync(UsersLoginFilterDto filter)
        {
            var query = _context.UsersLogins
                .AsNoTracking()
                .Include(x => x.Staff)
                .AsQueryable();

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);

            //if (filter.LoginType.HasValue)
            //    query = query.Where(x => x.LoginType == filter.LoginType);

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.LoginName.ToLower().Contains(search) ||
                    (x.ForgotEmail != null && x.ForgotEmail.ToLower().Contains(search)) ||
                    (x.Language != null && x.Language.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderBy(x => x.LoginId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UsersLoginRequestDto
                {
                    LoginId = x.LoginId,
                    StaffId = x.StaffId,
                    LoginName = x.LoginName,
                    Password = x.Password,
                    ForgotEmail = x.ForgotEmail,
                    IsActive = x.IsActive,
                    TempPassword = x.TempPassword,
                    TempPassDateTime = x.TempPassDateTime,
                    Language = x.Language,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy
                })
                .ToListAsync();

            return new PagedResultDto<UsersLoginRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



    }
}
