using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Label
{
    public class LabelFilterDto : BaseFilterDto
    {
        public long? ModuleId { get; set; }
    }
}
