using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRoleAssignment
{
    public class UsersRoleAssignmentRequestDto
    {
        public long? UserLoginRoleId { get; set; }   // null / 0 = Create, >0 = Update

        [Required(ErrorMessage = "Role is required")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public long LoginId { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }   // set server-side
        public long CreatedBy { get; set; }


        // For display purposes
        public string? LoginName { get; set; }
        public string? RoleName { get; set; }
        public string? Module { get; set; }
    }
}
