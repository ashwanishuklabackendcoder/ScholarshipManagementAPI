namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole
{
    public class LoginRoleAssignmentDto
    {
        public long RoleId { get; set; }
        public long LoginId { get; set; }

        public long? UserLoginRoleId { get; set; }

        public bool IsMapped { get; set; }
        public bool IsDefault { get; set; }

        public string? LoginName { get; set; }
        public string? RoleName { get; set; }
        public string? Module { get; set; }
    }
}
