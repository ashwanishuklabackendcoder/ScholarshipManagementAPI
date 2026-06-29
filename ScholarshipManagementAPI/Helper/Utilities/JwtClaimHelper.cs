using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.Helper.Enums;
using System.Reflection;
using System.Security.Claims;

namespace ScholarshipManagementAPI.Helper.Utilities
{
    public static class JwtClaimHelper
    {
        public static long LoginId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(JwtClaimTypes.LoginId)?.Value;

            if (string.IsNullOrWhiteSpace(claim))
                throw new UnauthorizedAccessException("LoginId claim missing");

            if (!long.TryParse(claim, out var loginId))
                throw new UnauthorizedAccessException("Invalid LoginId claim");

            return loginId;
        }

        public static long RoleId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(JwtClaimTypes.RoleId)?.Value;

            if (string.IsNullOrWhiteSpace(claim))
                throw new UnauthorizedAccessException("RoleId claim missing");

            if (!long.TryParse(claim, out var roleId))
                throw new UnauthorizedAccessException("Invalid RoleId claim");

            return roleId;
        }

        public static long ModuleId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(JwtClaimTypes.ModuleId)?.Value;

            if (string.IsNullOrWhiteSpace(claim))
                throw new UnauthorizedAccessException("ModuleId claim missing");

            if (!long.TryParse(claim, out var moduleId))
                throw new UnauthorizedAccessException("Invalid ModuleId claim");

            return moduleId;
        }

        public static string RoleName(ClaimsPrincipal user)
        {
            var value = user.FindFirst(JwtClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(value))
                throw new UnauthorizedAccessException("Role claim missing");

            return value;
        }

        // UserName == LoginName
        public static string UserName(ClaimsPrincipal user)
        {
            var value = user.FindFirst(JwtClaimTypes.UserName)?.Value;
            if (string.IsNullOrWhiteSpace(value))
                throw new UnauthorizedAccessException("UserName claim missing");

            return value;
        }


        public static LoggedInUserDto GetLoggedInUser(ClaimsPrincipal user)
        {
            var moduleId = ModuleId(user); // you already have this

            return new LoggedInUserDto
            {
                // existing claims (reuse your helpers)
                LoginId = LoginId(user),
                RoleId = RoleId(user),
                ModuleId = ModuleId(user),

                RoleName = RoleName(user),
                UserName = UserName(user),

                // staff type comes from module
                StaffType = (StaffType)moduleId,

                //UniversityId = GetNullableLongClaim(user, JwtClaimTypes.UniversityId),
                //SchoolId = GetNullableLongClaim(user, JwtClaimTypes.SchoolId),
                //NgoId = GetNullableLongClaim(user, JwtClaimTypes.NgoId)
            };
        }


    }
}
