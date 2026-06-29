using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginLog;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class UsersLoginLogService : IUsersLoginLogService
    {
        private readonly AppDbContext _context;

        public UsersLoginLogService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UsersLoginLogRequestDto dto)
        {
            var entity = new UsersLoginsLog
            {
                LoginId = dto.LoginId,
                IpAddress = dto.IpAddress,
                LoginDateTime = DateTime.UtcNow,    
                BrowserName = dto.BrowserName,
                OperatingSystem = dto.OperatingSystem,
                ComputerName = dto.ComputerName,
                UserName = dto.UserName

                // LoutDateTime is null on purpose
            };

            _context.UsersLoginsLogs.Add(entity);
            await _context.SaveChangesAsync();

            return entity.LoginLogId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UsersLoginLogRequestDto dto)
        {
            if (dto.LoginLogId == null || dto.LoginLogId == 0)
                return false;

            var entity = await _context.UsersLoginsLogs
                .FirstOrDefaultAsync(x => x.LoginLogId == dto.LoginLogId);

            if (entity == null)
                return false;


            entity.LogoutDateTime = dto.LogoutDateTime;

            // Other fields NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }




        // ---------------- GET BY ID ----------------
        public async Task<UsersLoginLogRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UsersLoginsLogs
                .AsNoTracking()
                .Include(x => x.Login)
                .Where(x => x.LoginLogId == id)
                .Select(x => new UsersLoginLogRequestDto
                {
                    LoginLogId = x.LoginLogId,
                    LoginId = x.LoginId,
                    IpAddress = x.IpAddress,
                    LoginDateTime = x.LoginDateTime,
                    LogoutDateTime = x.LogoutDateTime,
                    BrowserName = x.BrowserName,
                    OperatingSystem = x.OperatingSystem,
                    ComputerName = x.ComputerName,
                    UserName = x.UserName,

                    LoginName = x.Login.LoginName,
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UsersLoginLogRequestDto>> GetByFilterAsync(UsersLoginLogFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.UsersLoginsLogs
                .AsNoTracking()
                .Include(x => x.Login)
                .ThenInclude(l => l.Staff)
                .AsQueryable();

            // ---------- DATA SCOPE FILTER ----------
            if (currentUser.StaffType != StaffType.SuperAdmin)
            {
                if (currentUser.StaffType == (StaffType.University))
                {
                    query = query.Where(x => x.Login.Staff.UniversityId == currentUser.UniversityId);
                }
                else if (currentUser.StaffType == StaffType.School)
                {
                    query = query.Where(x => x.Login.Staff.SchoolId == currentUser.SchoolId);
                }
                else if (currentUser.StaffType == StaffType.Ngo)
                {
                    query = query.Where(x => x.Login.Staff.NgoId == currentUser.NgoId);
                }
            }


            if (filter.LoginId.HasValue)
                query = query.Where(x => x.LoginId == filter.LoginId);

            if (filter.LoginFrom.HasValue)
                query = query.Where(x => x.LoginDateTime >= filter.LoginFrom);

            if (filter.LoginTo.HasValue)
                query = query.Where(x => x.LoginDateTime <= filter.LoginTo);

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.IpAddress.ToLower().Contains(search) ||
                    (x.BrowserName != null && x.BrowserName.ToLower().Contains(search)) ||
                    (x.OperatingSystem != null && x.OperatingSystem.ToLower().Contains(search)) ||
                    (x.UserName != null && x.UserName.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.LoginDateTime);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UsersLoginLogRequestDto
                {
                    LoginLogId = x.LoginLogId,
                    LoginId = x.LoginId,
                    IpAddress = x.IpAddress,
                    LoginDateTime = x.LoginDateTime,
                    LogoutDateTime = x.LogoutDateTime,
                    BrowserName = x.BrowserName,
                    OperatingSystem = x.OperatingSystem,
                    ComputerName = x.ComputerName,
                    UserName = x.UserName,

                    LoginName = x.Login.LoginName,
                })
                .ToListAsync();

            return new PagedResultDto<UsersLoginLogRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
