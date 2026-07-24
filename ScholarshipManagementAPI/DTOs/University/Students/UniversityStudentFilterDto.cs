using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.Students
{
    public class UniversityStudentFilterDto : BaseFilterDto
    {
        public long UniversityId { get; set; }

        public long? FacultyId { get; set; }

        public long? ProgramId { get; set; }

        public long? StudentStatusId { get; set; }
    }
}
