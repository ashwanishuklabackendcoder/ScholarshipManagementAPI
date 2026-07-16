using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.StudentRegistration
{
    public class StudentRegistrationRequestDto
    {
        public long? StudentId { get; set; }

        // Section 1: Basic Info
        public IFormFile? Photo { get; set; }
        public string? PhotoPath { get; set; }

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? SecondName { get; set; }

        [MaxLength(200)]
        public string? ThirdName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? MotherName { get; set; }

        public DateTime? Dob { get; set; }

        [MaxLength(200)]
        public long? Nationality { get; set; }

        [MaxLength(200)]
        public long? ResidenceCountry { get; set; }

        public long? ResidenceCountryId { get; set; }

        [MaxLength(200)]
        public string? Tribe { get; set; }

        [MaxLength(200)]
        public string? Religion { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        public bool? IsOrphan { get; set; }

        [MaxLength(100)]
        public string? OrphanNumber { get; set; }

        // Section 2: Contact Info
        [MaxLength(200)]
        public string? City { get; set; }

        [MaxLength(200)]
        public string? Village { get; set; }

        [MaxLength(200)]
        public string? Block { get; set; }

        [MaxLength(200)]
        public string? Street { get; set; }

        [MaxLength(200)]
        public string? House { get; set; }

        [MaxLength(100)]
        public string? Phone { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? Email { get; set; }

        // Section 3: Academic Info
        public bool? FromDaSchool { get; set; }

        [MaxLength(100)]
        public string? DaStudentCode { get; set; }

        [MaxLength(300)]
        public string? SchoolName { get; set; }

        [MaxLength(200)]
        public string? HsSpecialization { get; set; }

        [MaxLength(300)]
        public string? CombinedSpec { get; set; }

        public decimal? TotalScore { get; set; }
        public decimal? MaxScore { get; set; }
        public decimal? RelativeGrade { get; set; }
        public decimal? EnglishScore { get; set; }

        // Transfer Student sub-section
        [MaxLength(300)]
        public string? TransferInstitution { get; set; }

        [MaxLength(300)]
        public string? TransferProgram { get; set; }

        [MaxLength(100)]
        public string? TransferInstitutionType { get; set; }

        public int? TransferCredits { get; set; }
        public DateTime? TransferLastSemEnd { get; set; }
        public decimal? TransferGpa { get; set; }

        // Section 4: Behavioral & Social Evaluation
        [MaxLength(100)]
        public string? FinancialNeed { get; set; }

        [MaxLength(50)]
        public string? SelfReliance { get; set; }

        [MaxLength(50)]
        public string? Motivation { get; set; }

        [MaxLength(50)]
        public string? FutureGoals { get; set; }

        public IFormFile? Recommendation { get; set; }
        public string? RecommendationLetterPath { get; set; }

        [MaxLength(2000)]
        public string? RecommendationLetterNotes { get; set; }

        public bool IsDraft { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }

    public class StudentRegistrationResponseDto
    {
        public long StudentId { get; set; }
        public string? PhotoPath { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? MotherName { get; set; }
        public DateTime? Dob { get; set; }
        public long? Nationality { get; set; }
        public long? ResidenceCountry { get; set; }
     
        public string? Tribe { get; set; }
        public string? Religion { get; set; }
        public string? Gender { get; set; }
        public bool? IsOrphan { get; set; }
        public string? OrphanNumber { get; set; }
        public string? City { get; set; }
        public string? Village { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? FromDaSchool { get; set; }
        public string? DaStudentCode { get; set; }
        public string? SchoolName { get; set; }
        public string? HsSpecialization { get; set; }
        public string? CombinedSpec { get; set; }
        public decimal? TotalScore { get; set; }
        public decimal? MaxScore { get; set; }
        public decimal? RelativeGrade { get; set; }
        public decimal? EnglishScore { get; set; }
        public string? TransferInstitution { get; set; }
        public string? TransferProgram { get; set; }
        public string? TransferInstitutionType { get; set; }
        public int? TransferCredits { get; set; }
        public DateTime? TransferLastSemEnd { get; set; }
        public decimal? TransferGpa { get; set; }
        public string? FinancialNeed { get; set; }
        public string? SelfReliance { get; set; }
        public string? Motivation { get; set; }
        public string? FutureGoals { get; set; }
        public string? RecommendationLetterPath { get; set; }
        public string? RecommendationLetterNotes { get; set; }
        public bool IsDraft { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
