namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRoleAssignment
{
    public class UsersRoleAssignmentSaveDto
    {
        public long LoginId { get; set; }

        public List<UsersRoleAssignmentDto> Roles { get; set; } = new();
    }
}
