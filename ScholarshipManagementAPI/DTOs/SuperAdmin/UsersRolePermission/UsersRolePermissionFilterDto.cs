using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePermission
{
    public class UsersRolePermissionFilterDto:BaseFilterDto
    {
        /* -------- RolePage specific -------- */
        public long? RoleId { get; set; }
        public long? MenuLinkId { get; set; }



        /* -------- Role based filters -------- */
        public long? ModuleId { get; set; }
        public bool? IsActive { get; set; }
        public long? DashboardMenuLinkId { get; set; }
    }
}
