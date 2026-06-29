using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage
{
    public class UsersRolePageRequestDto
    {
        public long? RoleFormId { get; set; } // null/0 = Create, >0 = Update

        [Required(ErrorMessage = "Role is required")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "Menu is required")]
        public long MenuLinkId { get; set; }

        public bool InsertPer { get; set; }

        public bool UpdatePer { get; set; }

        public bool DeletePer { get; set; }

        public bool ViewPer { get; set; }




        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }


        // ony for response purpose
        public string? RoleName { get; set; }
        public string? PageHeading { get; set; }
        public string? PagePath { get; set; }
        public string? Module { get; set; }

    }

}
