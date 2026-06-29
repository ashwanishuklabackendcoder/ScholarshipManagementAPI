namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole
{
    public class LoginRoleBulkSaveDto
    {
        public long LoginId { get; set; }

        public List<LoginRoleAssignmentDto> Roles { get; set; } = new();
    }
}
