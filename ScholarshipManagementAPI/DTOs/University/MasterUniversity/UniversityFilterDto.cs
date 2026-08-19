using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.MasterUniversity
{
    public class UniversityFilterDto:BaseFilterDto
    {
        public long? UniversityId { get; set; }

        public long? CountryId { get; set; }

        public long? UniversityTypeId { get; set; }

        public long? StudentsGenderTypeId { get; set; }

        public byte? AccreditationStatus { get; set; }


    }
}
