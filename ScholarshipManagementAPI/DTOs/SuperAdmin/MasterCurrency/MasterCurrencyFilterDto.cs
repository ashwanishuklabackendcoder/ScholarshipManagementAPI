using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry
{
    public class MasterCurrencyFilterDto :BaseFilterDto
    {
        public string? CurrencyName { get; set; }

        public string? CurrencyCode { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
