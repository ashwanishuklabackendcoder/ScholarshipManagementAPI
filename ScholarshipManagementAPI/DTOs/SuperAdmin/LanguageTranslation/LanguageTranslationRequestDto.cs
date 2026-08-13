using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation
{
    public class LanguageTranslationRequestDto
    {
        public long? TranslationId { get; set; }

        [Required(ErrorMessage = "Label is required")]
        public long LabelId { get; set; }

        [Required(ErrorMessage = "Language is required")]
        public long LanguageId { get; set; }

        [Required(ErrorMessage = "Translation value is required")]
        [StringLength(1000, ErrorMessage = "Translation value cannot exceed 1000 characters")]
        public string LabelValue { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // Display information
        public string? LabelKey { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageCode { get; set; }

        // Server controlled
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
    }
}
