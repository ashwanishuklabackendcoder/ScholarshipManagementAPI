using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings
{
    public class GeneralSettingFilterDto:BaseFilterDto
    {
        public string? ConfigKey { get; set; }
        public string? ConfigValue { get; set; }
    }
}
