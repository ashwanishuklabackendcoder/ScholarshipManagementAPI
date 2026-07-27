using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.Ngo.StudentCategory
{
    public class StudentCategoryFilterDto : BaseFilterDto
    {
        public long? StudentCategoryId { get; set; }

        public bool? IsActive { get; set; }
    }
}
