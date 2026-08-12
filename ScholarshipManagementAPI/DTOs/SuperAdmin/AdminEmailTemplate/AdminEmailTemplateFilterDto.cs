using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.AdminEmailTemplate
{
    public class AdminEmailTemplateFilterDto : BaseFilterDto
    {
        public bool? IsActive { get; set; }
    }
}
