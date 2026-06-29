using System.Security.Claims;

namespace ScholarshipManagementAPI.Helper.Utilities
{
    public static class JwtClaimTypes
    {
        // Custom claims
        public const string LoginId = "LoginId";
        public const string RoleId = "RoleId";
        public const string ModuleId = "ModuleId";

        // Standard (JWT / ASP.NET understands these)
        public const string Role = ClaimTypes.Role;
        public const string UserName = ClaimTypes.Name;
    }
}
