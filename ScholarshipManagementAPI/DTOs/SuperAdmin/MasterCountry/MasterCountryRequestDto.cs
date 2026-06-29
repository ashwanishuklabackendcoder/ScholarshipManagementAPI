using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry
{
    public class MasterCountryRequestDto
    {
        public long? CountryId { get; set; }   // null / 0 = Create, >0 = Update

        [Required(ErrorMessage = "Country name is required")]
        [StringLength(200, ErrorMessage = "Country name cannot exceed 200 characters")]
        public string CountryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISD code is required")]
        public int CountryIsdCode { get; set; }

        [StringLength(5, ErrorMessage = "Alpha code cannot exceed 5 characters")]
        public string? CountryAlphaCode3 { get; set; }

        [StringLength(50, ErrorMessage = "Currency name cannot exceed 50 characters")]
        public string? CurrencyName { get; set; }

        [StringLength(250, ErrorMessage = "Currency fractional unit cannot exceed 250 characters")]
        public string? CurrencyFracUnit { get; set; }

        [StringLength(250, ErrorMessage = "Currency symbol cannot exceed 250 characters")]
        public string? CurrencySymbol { get; set; }

        [StringLength(10, ErrorMessage = "Currency abbreviation cannot exceed 10 characters")]
        public string? CurrencyAbb { get; set; }

        public bool IsActive { get; set; }
    }
}
