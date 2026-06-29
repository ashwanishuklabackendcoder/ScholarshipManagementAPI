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

        [Required]
        public int LevelNo { get; set; }

        [Required]
        public int SequenceNo { get; set; }

        public string? Icon { get; set; }

        public bool IsDashboard { get; set; }

        [Required]
        public bool ShowInMenu { get; set; }


        [Required]
        public DateTime CreatedDate { get; set; }


        public string? CreatedBy { get; set; }



        // Additional properties for response purposes
        public string? ModuleName { get; set; }
        public string? ParentName { get; set; }
    }
}
