using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.Programs
{
    public class ProgramFilterDto : BaseFilterDto
    {
        public long? ProgramId { get; set; }

        public long? UniversityId { get; set; }

        public long? FacultyId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDraft { get; set; }

        public byte? AccreditationStatus { get; set; }
    }
}
