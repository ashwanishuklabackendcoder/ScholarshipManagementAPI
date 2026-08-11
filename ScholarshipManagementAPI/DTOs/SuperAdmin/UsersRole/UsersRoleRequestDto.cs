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

        public bool IsActive { get; set; }



        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }


        // required for display purposes
        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }

        public string? ModuleName { get; set; }

    }
}
