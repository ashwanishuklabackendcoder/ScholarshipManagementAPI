namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole
{
    public class UsersRoleLookupDto
    {
        public long RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public long ModuleId { get; set; }

        public string ModuleName { get; set; } = string.Empty;
    }
}
