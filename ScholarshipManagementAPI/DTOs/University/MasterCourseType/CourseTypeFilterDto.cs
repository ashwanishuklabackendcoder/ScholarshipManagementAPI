using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.MasterCourseType
{
    public class CourseTypeFilterDto : BaseFilterDto
    {
        public long? UniversityId { get; set; }
        public int? ApprovalStatus { get; set; }
        public bool? IsActive { get; set; }

    }
}
