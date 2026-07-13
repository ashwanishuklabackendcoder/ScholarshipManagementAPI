using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.School.StudentRegistration
{
    public class StudentRegistrationFilterDto : BaseFilterDto
    {
        public bool? IsDraft { get; set; }
        public bool? IsActive { get; set; }
        public string? Gender { get; set; }
        public string? SchoolName { get; set; }
    }
}
