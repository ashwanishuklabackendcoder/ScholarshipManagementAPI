using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.Faculties
{
    public class FacultyFilterDto : BaseFilterDto
    {
        public long? FacultyId { get; set; }

        public long? UniversityId { get; set; }

        public bool? IsActive { get; set; }

    }
}
