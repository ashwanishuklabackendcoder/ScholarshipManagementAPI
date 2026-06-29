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

        [StringLength(250, ErrorMessage = "Fraction unit cannot exceed 250 characters")]
        public string? CurrencyFracUnit { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }   // Set on Create only
    }
}
