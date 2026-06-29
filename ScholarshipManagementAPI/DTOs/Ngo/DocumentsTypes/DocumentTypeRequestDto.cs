using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.Ngo.DocumentsTypes
{
    public class DocumentTypeRequestDto
    {
        public long? DocumentTypeId { get; set; }         // For Update scenarios - Optional for Create, Required for Update

        [Required]
        [StringLength(200)]
        public string DocumentName { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public bool DefaultRequired { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;


        // Response only
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
    }
}
