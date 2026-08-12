using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown
{
    public class MasterDropDownFilterDto : BaseFilterDto
    {
        public long? ParentId { get; set; }
        public bool? IsActive { get; set; }
    }
}
