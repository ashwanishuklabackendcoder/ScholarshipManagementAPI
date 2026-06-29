using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.Ngo.Donor
{
    public class DonorRequestDto
    {
        public long? DonorId { get; set; }               // null / 0 = Create, >0 = Update

        // Required + max length
        [Required(ErrorMessage = "Donor Name is required")]
        [MaxLength(200, ErrorMessage = "Donor Name cannot exceed 200 characters")]
        public string DonorName { get; set; } = string.Empty;

        // Required
        [Required(ErrorMessage = "Donor Code is required")]
        [MaxLength(50, ErrorMessage = "Donor Code cannot exceed 50 characters")]
        public string DonorCode { get; set; } = string.Empty;

        // Optional
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? DonorEmail { get; set; }

        // Required
        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        public string DonorPhone { get; set; } = string.Empty;

        // Optional
        public string? Remarks { get; set; }


       

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }




        // response

        public long? AssociatedStudentCount { get; set; }

    }
}
