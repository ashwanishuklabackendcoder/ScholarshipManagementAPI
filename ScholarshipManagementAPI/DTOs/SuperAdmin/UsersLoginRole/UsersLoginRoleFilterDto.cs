using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole
{
    public class UsersLoginRoleFilterDto : BaseFilterDto
    {
        public long? RoleId { get; set; }
        public long? LoginId { get; set; }
        public bool? IsDefault { get; set; }
    }
}
