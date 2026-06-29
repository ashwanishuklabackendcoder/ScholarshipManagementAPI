namespace ScholarshipManagementAPI.DTOs.Common.Settings
{
    public class AvailableRolesDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public long ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }
}
