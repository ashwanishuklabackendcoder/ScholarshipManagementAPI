using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings
{
    public class GeneralSettingRequestDto
    {
        public long? ConfigId { get; set; }   // null/0 = Create, >0 = Update

        [Required(ErrorMessage = "Config key is required")]
        [StringLength(200, ErrorMessage = "Config key cannot exceed 200 characters")]
        public string ConfigKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Config value is required")]
        [StringLength(200, ErrorMessage = "Config value cannot exceed 200 characters")]
        public string ConfigValue { get; set; } = string.Empty;


        [StringLength(500, ErrorMessage = "Config description cannot exceed 500 characters")]
        public string? ConfigDescription { get; set; }



        public bool IsActive { get; set; }


        // These should usually be server-controlled
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }


        // required for display purposes
        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }


    }
}
