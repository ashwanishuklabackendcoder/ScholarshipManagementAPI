using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.School.Students
{
    public class StudentFilterDto : BaseFilterDto
    {
        public long? StudentId { get; set; }
        public long? SchoolId { get; set; }

        public string? StudentNumber { get; set; }


    }
}
