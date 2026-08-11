using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRoleAssignment
{
    public class UsersRoleAssignmentFilterDto : BaseFilterDto
    {
        public long? RoleId { get; set; }
        public long? LoginId { get; set; }
        public bool? IsDefault { get; set; }
    }
}
