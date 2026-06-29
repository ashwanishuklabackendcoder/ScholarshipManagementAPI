using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole
{
    public class UsersRoleFilterDto : BaseFilterDto
    {
        public long? ModuleId { get; set; }
        public long? DashboardMenuLinkId { get; set; }
        public bool? IsActive { get; set; }
    }
}
