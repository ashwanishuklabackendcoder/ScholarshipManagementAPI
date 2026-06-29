using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown
{
    public class MasterDropDownFilterDto : BaseFilterDto
    {
        public long? ModuleId { get; set; }
        public long? ParentId { get; set; }
        public bool? Status { get; set; }
        public bool? IsShow { get; set; }
      
    }
}
