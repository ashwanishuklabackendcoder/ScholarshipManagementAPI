namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePermission
{
    public class UsersRolePermissionBulkSaveDto
    {
        public long RoleId { get; set; }
        public List<UsersRolePermissionDto> Permissions { get; set; } = new();
    }
}
