using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.CourseRequirement
{
    public class CourseRequirementRequestDto
    {

        public long? ReqId { get; set; }  // null for create, value for update

        [Required]
        public long CourseId { get; set; }

        [Required]
        [StringLength(50)]
        public string AcademicYear { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string RequiredDocuments { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string HsDivision { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "NoSeats must be greater than 0")]
        public int NoSeats { get; set; }

        
        public double? CollegeScore { get; set; }

        [StringLength(50)]
        public string? TzStuCombi { get; set; }

        
        public double? RegistrationCost { get; set; }

        
        public double? TutionCost { get; set; }

        
        public double? TextBookCost { get; set; }

        
        public double? AccomoCost { get; set; }

        
        public double? TravellingCost { get; set; }

        
        public double? TransportCost { get; set; }

        
        public double? DocuAttestCost { get; set; }

        
        public double? VisaResiCost { get; set; }

        public DateTime? ReqStartDate { get; set; }
        public DateTime? ReqEndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;


        public int ApprovalStatus { get; set; }
        public long? ApprovedBy { get; set; }

        // for response 

        public string? CourseName { get; set; }
        public string? ApprovedByName { get; set; }
        public string? UniversityName { get; set; }

    }
}
