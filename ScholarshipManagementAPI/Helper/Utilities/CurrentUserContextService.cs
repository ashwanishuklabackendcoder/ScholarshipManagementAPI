using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Helper.Utilities
{
    public class CurrentUserContextService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneralSettingsService _generalSettingsService;

        public CurrentUserContextService(   
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IGeneralSettingsService generalSettingsService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _generalSettingsService = generalSettingsService;
        }

        public async Task<LoggedInUserDto> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext!.User;

            // Read JWT claims (fast)
            var loginId = JwtClaimHelper.LoginId(user);
            var roleId = JwtClaimHelper.RoleId(user);
            var moduleId = JwtClaimHelper.ModuleId(user);

            if (!Enum.IsDefined(typeof(StaffType), (int)moduleId))
                throw new UnauthorizedAccessException("Invalid module");

            // Fetch tenant mapping ONCE (DB)
            var staffInfo = await _context.UsersLogins
                .Include(x => x.Staff) // eager load staff for currency lookup
                .AsNoTracking()
                .Where(x => x.LoginId == loginId)
                .Select(x => new
                {
                    x.StaffId,
                    x.Staff.StaffType,
                    x.Staff.UniversityId,
                    x.Staff.SchoolId,
                    x.Staff.NgoId,
                    x.Staff
                })
                .FirstOrDefaultAsync();

            if (staffInfo == null)
                throw new UnauthorizedAccessException("Staff mapping not found");


            var currency = await GetDefaultCurrencyAsync(staffInfo.Staff);

            // Build context
            return new LoggedInUserDto
            {
                LoginId = loginId,
                RoleId = roleId,
                ModuleId = moduleId,

                RoleName = JwtClaimHelper.RoleName(user),
                UserName = JwtClaimHelper.UserName(user),

                StaffType = (StaffType)moduleId,

                StaffId = staffInfo.StaffId,
                UniversityId = staffInfo.UniversityId,
                SchoolId = staffInfo.SchoolId,
                NgoId = staffInfo.NgoId,

                DefaultCurrencyCode = currency.code,
                DefaultCurrencyName = currency.name,
                DefaultCurrencySymbol = currency.symbol
            };
        }


        // helper method to get default currency based on staff's organization or fallback to base currency
        private async Task<(string code, string name, string symbol)> GetDefaultCurrencyAsync(HrStaffMaster staff)
        {
            var config = await _generalSettingsService.GetGeneralConfigAsync();

            var baseCode = config.BaseCurrencyCode;
            var baseName = config.BaseCurrencyName;
            var baseSymbol = config.BaseCurrencySymbol;

            long? currencyId = null;

            // 🔹 UNIVERSITY
            if (staff.StaffType == (long)StaffType.University && staff.UniversityId.HasValue)
            {
                currencyId = null;
            }

            // 🔹 SCHOOL
            else if (staff.StaffType == (long)StaffType.School && staff.SchoolId.HasValue)
            {
                currencyId = await _context.KfSchools
                    .Where(s => s.SchoolId == staff.SchoolId.Value)
                    .Select(s => s.DefaultCurrencyId)
                    .FirstOrDefaultAsync();
            }

            // 🔹 If currency found → fetch details
            if (currencyId.HasValue)
            {
                var currency = await _context.ZzMasterCurrencies
                    .Where(x => x.CurrencyId == currencyId.Value)
                    .Select(x => new { x.CurrencyCode, x.CurrencyName, x.CurrencySymbol })
                    .FirstOrDefaultAsync();

                if (currency != null)
                {
                    return (currency.CurrencyCode, currency.CurrencyName, currency.CurrencySymbol);
                }
            }

            // 🔹 Fallback → Base currency
            return (baseCode, baseName, baseSymbol);
        }




    }
}
