namespace ScholarshipManagementAPI.DTOs.Common.Auth
{
    public class TokenDto
    {
        public long LoginId { get; set; }
        public string LoginName { get; set; }= string.Empty;

        public long RoleId{ get; set; }
        public string RoleName { get; set; } = string.Empty;

        public long ModuleId { get; set; }
    }
}
