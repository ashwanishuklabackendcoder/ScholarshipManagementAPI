using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ScholarshipManagementAPI.DTOs.School.Students
{
    public class StudentRequestDto
    {
        public long? StudentId { get; set; }  // null / 0 = Create, >0 = Update

        [Required]
        public long SchoolId { get; set; }

        public string? StudentNumber { get; set; }

        [StringLength(100)]
        public string? StudentSalutation { get; set; }

        [Required]
        [StringLength(500)]
        public string StudentFirstName { get; set; } = null!;

        [StringLength(500)]
        public string? StudentLastName { get; set; }

        [StringLength(200)]
        public string? StudentOtherName { get; set; }

        [StringLength(200)]
        public string? NIN { get; set; }

        public DateTime? DateOfBirth { get; set; }

       
        public int? Gender { get; set; }

        [StringLength(200)]
        public string? Tribe { get; set; }

        [StringLength(200)]
        public string? Nationality { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(200)]
        public string? AddressCity { get; set; }

        [StringLength(200)]
        public string? MasterState { get; set; }

        [StringLength(200)]
        public string? MasterCountry { get; set; }

        [StringLength(50)]
        public string? ZipCode { get; set; }

        [StringLength(500)]
        [Phone]
        public string? MobileNo { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public string? EmailID { get; set; }

        [StringLength(1000)]
        public string? Photo { get; set; }

        public bool? IsOrphan { get; set; }

        [StringLength(500)]
        public string? OrphanNumber { get; set; }

        public long? Religion { get; set; }

        [StringLength(50)]
        public string? GraduationScore { get; set; }

        [StringLength(50)]
        public string? Grade { get; set; }

        [StringLength(50)]
        public string? HighSchoolDiv { get; set; }

        [StringLength(50)]
        public string? TanzComb { get; set; }

        [StringLength(200)]
        public string? FatherName { get; set; }

        [StringLength(200)]
        public string? MotherName { get; set; }

        [StringLength(200)]
        public string? GuardianName { get; set; }

        [StringLength(200)]
        public string? SocialEcoStatus { get; set; }

        [StringLength(200)]
        public string? RecommendationLetter { get; set; }

        [StringLength(200)]
        public string? SelfDettoSuccess { get; set; }

        [StringLength(200)]
        public string? MotLevelToOverComedStudying { get; set; }

        [StringLength(200)]
        public string? ClearTargetsFutureGoals { get; set; }

        [Range(0, 999999)]
        public decimal? MaxMarks { get; set; }

        [Range(0, 999999)]
        public decimal? EnglishPlacementTest { get; set; }

        [JsonIgnore]
        public IFormFile? RecommendationLetterFile { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }


        // RESPOSNE

        public string? SchoolName { get; set; }
        public string? ShortName { get; set; }

        public string? FullName { get; set; }
        public string? FormatedCreatedBy { get; set; }
        public string? FormattedCreatedDate { get; set; }

        public string? RecommendationLetterPath { get; set; }

    }
}
