using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.AdminEmailTemplate
{
    public class AdminEmailTemplateFilterDto : BaseFilterDto
    {
        public long? SchoolId { get; set; }
        public bool? IsActive { get; set; }
    }
}
