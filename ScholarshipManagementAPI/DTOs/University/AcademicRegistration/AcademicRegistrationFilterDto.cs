using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.AcademicRegistration
{
    public class AcademicRegistrationFilterDto : BaseFilterDto
    {
        public long? UniversityId { get; set; }

        public long? FacultyId { get; set; }

        public long? ProgramId { get; set; }

        public int? SemesterNo { get; set; }

        public bool? RegisteredOnly { get; set; }
    }
}
