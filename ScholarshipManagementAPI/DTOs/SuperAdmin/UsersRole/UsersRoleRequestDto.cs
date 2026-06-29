using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole
{
    public class UsersRoleRequestDto
    {
        public long? RoleId { get; set; }   // null / 0 = Create, >0 = Update

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(200, ErrorMessage = "Role name cannot exceed 200 characters")]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Module is required")]
        public long ModuleId { get; set; }

        public long? DashboardMenuLinkId { get; set; }   // FK → ZzUsersMenu

        [Required]
        public bool IsActive { get; set; }



        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }



        // required for display purposes
        public string? ModuleName { get; set; }
        public string? DashboardMenuName { get; set; }
        public string? DashboardPath { get; set; }
    }
}
