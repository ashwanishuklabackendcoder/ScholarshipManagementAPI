using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu
{
    public class UsersMenuRequestdto
    {
        public long? MenuLinkId { get; set; }   // null/0 = Create, >0 = Update

        [Required(ErrorMessage = "Module is required")]
        public long ModuleId { get; set; }

        [Required(ErrorMessage = "Page heading is required")]
        [StringLength(200)]
        public string PageHeading { get; set; } = string.Empty;

        public long? ParentId { get; set; }

        [Required(ErrorMessage = "Page path is required")]
        [StringLength(200)]
        public string PagePath { get; set; } = string.Empty;

        [Required(ErrorMessage = "Actual name is required")]
        [StringLength(200)]
        public string ActualName { get; set; } = string.Empty;

        [Required]
        public bool IsView { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int LevelNo { get; set; }

        [Required]
        public int SequenceNo { get; set; }

        public string? Icon { get; set; }

        
        
        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public string? CreatedByName { get; set; }


        public DateTime? UpdatedDate { get; set; }

        public long? UpdatedBy { get; set; }

        public string? UpdatedByName { get; set; }


        // Additional properties for response purposes
        public string? ModuleName { get; set; }
        public string? ParentName { get; set; }

    }
}
