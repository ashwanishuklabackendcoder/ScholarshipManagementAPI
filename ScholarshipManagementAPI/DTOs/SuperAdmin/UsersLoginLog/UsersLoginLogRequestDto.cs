using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginLog
{
    public class UsersLoginLogRequestDto
    {
        public long? LoginLogId { get; set; }   // null / 0 = Create, >0 = Update

        [Required(ErrorMessage = "Login is required")]
        public long LoginId { get; set; }

        [Required(ErrorMessage = "IP Address is required")]
        [StringLength(200)]
        public string IpAddress { get; set; } = string.Empty;

        [Required]
        public DateTime LoginDateTime { get; set; }

        public DateTime? LogoutDateTime { get; set; }

        [StringLength(200)]
        public string? BrowserName { get; set; }

        [StringLength(200)]
        public string? OperatingSystem { get; set; }

        [StringLength(200)]
        public string? ComputerName { get; set; }

        [StringLength(200)]
        public string? UserName { get; set; }



        // for display purpose
        public string? LoginName { get; set; }
    }
}
