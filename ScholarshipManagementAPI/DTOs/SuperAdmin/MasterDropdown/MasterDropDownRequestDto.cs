using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperADmin.ZzMasterDropdown
{
    public class MasterDropDownRequestDto
    {
        public long? UniqueId { get; set; }   // null or 0 = create

        [Required(ErrorMessage = "DisplayText is required")]
        [StringLength(500)]
        public string DisplayText { get; set; } = null!;

        public long? ParentId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "DisplaySequence must be >= 0")]
        public int DisplaySequence { get; set; }

        public bool IsActive { get; set; }


        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }


        // required for display purposes
        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }


        public string? ParentName { get; set; }
    }
}
