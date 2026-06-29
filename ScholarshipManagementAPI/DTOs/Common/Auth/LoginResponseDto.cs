using ScholarshipManagementAPI.DTOs.Common.Settings;

namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class LoginResponseDto
    {
        // Auth
        public string Token { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }

        // User identity
        public long LoginId { get; set; }
        public string LoginName { get; set; } = string.Empty;

        // Module context
        public long ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;

        // Role context
        public long CurrentRoleId { get; set; }
        public string CurrentRoleName { get; set; } = string.Empty;
        public List<AvailableRolesDto> AvailableRoles { get; set; } = new();


        //public long RoleId { get; set; }



        // Security flow flags
        // public bool MustChangePassword { get; set; }   // first time login
        // public string? Language { get; set; }          // language
    }
}
