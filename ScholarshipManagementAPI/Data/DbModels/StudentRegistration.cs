using System;

namespace ScholarshipManagementAPI.Data.DbModels
{
    public class StudentRegistration
    {
        public long StudentId { get; set; }
        
        // Section 1: Basic Info
        public string? PhotoPath { get; set; }
        public string FirstName { get; set; } = null!;
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = null!;
        public string? MotherName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Nationality { get; set; }
        public string? ResidenceCountry { get; set; }
        public long? ResidenceCountryId { get; set; }
        public string? Tribe { get; set; }
        public string? Religion { get; set; }
        public string? Gender { get; set; }
        public bool? IsOrphan { get; set; }
        public string? OrphanNumber { get; set; }
        
        // Section 2: Contact Info
        public string? City { get; set; }
        public string? Village { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        
        // Section 3: Academic Info
        public bool? FromDaSchool { get; set; }
        public string? DaStudentCode { get; set; }
        public string? SchoolName { get; set; }
        public string? HsSpecialization { get; set; }
        public string? CombinedSpec { get; set; }
        public decimal? TotalScore { get; set; }
        public decimal? MaxScore { get; set; }
        public decimal? RelativeGrade { get; set; }
        public decimal? EnglishScore { get; set; }
        
        // Transfer Student sub-section
        public string? TransferInstitution { get; set; }
        public string? TransferProgram { get; set; }
        public string? TransferInstitutionType { get; set; }
        public int? TransferCredits { get; set; }
        public DateTime? TransferLastSemEnd { get; set; }
        public decimal? TransferGpa { get; set; }
        
        // Section 4: Behavioral & Social Evaluation
        public string? FinancialNeed { get; set; }
        public string? SelfReliance { get; set; }
        public string? Motivation { get; set; }
        public string? FutureGoals { get; set; }
        public string? RecommendationLetterPath { get; set; }
        public string? RecommendationLetterNotes { get; set; }

        // Audit fields
        public bool IsDraft { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public long CreatedBy { get; set; } = 2;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ZzMasterCountry? ResidenceCountryNavigation { get; set; }
    }
}
