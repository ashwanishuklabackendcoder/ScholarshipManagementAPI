using ScholarshipManagementAPI.DTOs.Common.Filter;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.MasterCourse
{
    public class MasterCourseFilterDto : BaseFilterDto
    {
        public long? UniversityId { get; set; }
        public long? CourseTypeId { get; set; }
        public int? ApprovalStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
