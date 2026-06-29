namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage
{
    public class RolePermissionBulkSaveDto
    {
        public long RoleId { get; set; }
        public List<RolePagePermissionDto> Permissions { get; set; } = new();
    }
}
