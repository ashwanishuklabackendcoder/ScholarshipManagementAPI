using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginLog
{
    public class UsersLoginLogFilterDto : BaseFilterDto
    {
        public long? LoginId { get; set; }

        public DateTime? LoginFrom { get; set; }
        public DateTime? LoginTo { get; set; }
    }
}
