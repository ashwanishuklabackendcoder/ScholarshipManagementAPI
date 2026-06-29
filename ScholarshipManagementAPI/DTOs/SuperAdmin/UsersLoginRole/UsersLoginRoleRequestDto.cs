using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole
{
    public class UsersLoginRoleRequestDto
    {
        public long? UserLoginRoleId { get; set; }   // null / 0 = Create, >0 = Update

        [Required(ErrorMessage = "Role is required")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public long LoginId { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }   // set server-side
        public string CreatedBy { get; set; } = string.Empty;


        // For display purposes
        public string? LoginName { get; set; }
        public string? RoleName { get; set; }
        public string? Module { get; set; }
    }
}
