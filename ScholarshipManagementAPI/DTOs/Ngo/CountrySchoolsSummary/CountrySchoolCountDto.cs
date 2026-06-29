namespace ScholarshipManagementAPI.DTOs.Ngo.CountrySchoolsSummary
{
    public class CountrySchoolCountDto
    {
        public long CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? CountryIsdCode { get; set; }
        public int TotalSchools { get; set; }
    }
}
