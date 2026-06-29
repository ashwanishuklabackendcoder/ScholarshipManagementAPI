using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes
{
    public class SponsorshipTypeRequestDto
    {
        public long? SponsorshipTypeId { get; set; }                     // For Update scenarios - Optional for Create, Required for Update

        [Required]
        [StringLength(200)]
        public string SponsorshipName { get; set; } = string.Empty;

        [Required]
        public byte FrequencyType { get; set; }

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
