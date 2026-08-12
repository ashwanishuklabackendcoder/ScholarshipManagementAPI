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


        public bool IsActive { get; set; }


        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }


        // required for display purposes
        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }
    }
}
