using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.AdminEmailTemplate
{
    public class AdminEmailTemplateRequestDto
    {
        
        public long? EmailTempId { get; set; }   // null/0 = Create, >0 = Update
       
        public long? SchoolID { get; set; }   

        [Required(ErrorMessage = "Template name is required.")]
        [StringLength(200, ErrorMessage = "Template name cannot exceed 200 characters.")]
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email subject is required.")]
        [StringLength(1000, ErrorMessage = "Subject cannot exceed 1000 characters.")]
        [Display(Name = "Email Subject")]
        public string Subject { get; set; } = string.Empty;

        [Display(Name = "Email Body")]
        public string? Template { get; set; }

        [Required]
        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;


        public DateTime CreatedDate { get; set; }   // server-side preferred


    }
}
