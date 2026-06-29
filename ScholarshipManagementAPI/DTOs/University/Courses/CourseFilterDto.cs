using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.Courses
{
    public class CourseFilterDto : BaseFilterDto
    {
        public long? CourseId { get; set; }

        public long? UniversityId { get; set; }

        public long? FacultyId { get; set; }

        public bool? IsActive { get; set; }
    }
}
