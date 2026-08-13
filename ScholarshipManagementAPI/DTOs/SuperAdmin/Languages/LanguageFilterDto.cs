using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Languages
{
    public class LanguageFilterDto : BaseFilterDto
    {
        public bool? IsDefault { get; set; }
        public bool? IsRtl { get; set; }
    }
}
