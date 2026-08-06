namespace ScholarshipManagementAPI.DTOs.School.MasterSchool
{
    public class SchoolByCountryRequestDto
    {
        public List<long> CountryIds { get; set; } = new();
    }
}
