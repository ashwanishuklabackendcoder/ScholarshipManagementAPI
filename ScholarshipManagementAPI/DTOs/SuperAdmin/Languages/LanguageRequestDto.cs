using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Languages
{
    public class LanguageRequestDto
    {
        public long? LanguageId { get; set; }

        [Required(ErrorMessage = "Language name is required")]
        [StringLength(100, ErrorMessage = "Language name cannot exceed 100 characters")]
        public string LanguageName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language code is required")]
        [StringLength(10, ErrorMessage = "Language code cannot exceed 10 characters")]
        public string LanguageCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Culture code is required")]
        [StringLength(20, ErrorMessage = "Culture code cannot exceed 20 characters")]
        public string CultureCode { get; set; } = string.Empty;

        public bool IsRTL { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        // Server controlled
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }

        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
    }
}
