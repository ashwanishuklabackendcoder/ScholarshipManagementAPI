using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.MasterUniversity
{
    public class UniversityFilterDto:BaseFilterDto
    {
        public long? RegistrationId { get; set; }

        public string? UniversityName { get; set; }

        public long? CountryId { get; set; }

        public long? UniversityTypeId { get; set; }

        public long? StudentsGenderTypeId { get; set; }

        public int? AccreditationStatus { get; set; }

        public long? AccreditationBy { get; set; }

        public bool? IsDraft { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public DateTime? AccreditationFrom { get; set; }

        public DateTime? AccreditationTo { get; set; }

    }
}
