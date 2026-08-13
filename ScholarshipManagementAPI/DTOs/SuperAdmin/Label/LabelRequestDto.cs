using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Label
{
    public class LabelRequestDto
    {
        public long? LabelId { get; set; }   // 0 = Create, >0 = Update

        public long? ModuleId { get; set; }

        [Required(ErrorMessage = "Label key is required")]
        [StringLength(200, ErrorMessage = "Label key cannot exceed 200 characters")]
        public string LabelKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Label value is required")]
        [StringLength(1000, ErrorMessage = "Label value cannot exceed 1000 characters")]
        public string LabelValue { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // Server controlled
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public string? ModuleName { get; set; }
    }
}
