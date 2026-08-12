using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry
{
    public class MasterCurrencyRequestDto
    {
        public long? CurrencyId { get; set; }   // null or 0 = Create, >0 = Update

        [Required(ErrorMessage = "Currency name is required")]
        [StringLength(50, ErrorMessage = "Currency name cannot exceed 50 characters")]
        public string CurrencyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Currency code is required")]
        [StringLength(10, ErrorMessage = "Currency code cannot exceed 10 characters")]
        public string CurrencyCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Currency symbol is required")]
        [StringLength(10, ErrorMessage = "Currency symbol cannot exceed 10 characters")]
        public string CurrencySymbol { get; set; } = string.Empty;


        public long CountryId { get; set; } 

        public bool IsActive { get; set; }


        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }


        // required for display purposes
        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }

        public string? CountryName { get; set; }
    }
}
