using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfStudentRegistration
{
    public long StudentId { get; set; }

    public string? PhotoPath { get; set; }

    public string FirstName { get; set; } = null!;

    public string? SecondName { get; set; }

    public string? ThirdName { get; set; }

    public string LastName { get; set; } = null!;

    public string? MotherName { get; set; }

    public DateOnly? Dob { get; set; }

    public long? NationalityId { get; set; }

    public long? ResidenceCountryId { get; set; }

    public string? Tribe { get; set; }

    public long? ReligionId { get; set; }

    public long? GenderId { get; set; }

    public bool? IsOrphan { get; set; }

    public string? OrphanNumber { get; set; }

    public string? City { get; set; }

    public string? Village { get; set; }

    public string? Block { get; set; }

    public string? Street { get; set; }

    public string? House { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool FromDaSchool { get; set; }

    public string? DaStudentCode { get; set; }

    public long SchoolId { get; set; }

    public string? HsSpecialization { get; set; }

    public string? TanzanianStudentCombination { get; set; }

    public decimal? TotalScore { get; set; }

    public decimal? MaxScore { get; set; }

    public decimal? RelativeGrade { get; set; }

    public decimal? EnglishScore { get; set; }

    public string? TransferInstitution { get; set; }

    public string? TransferProgram { get; set; }

    public string? TransferInstitutionType { get; set; }

    public int? TransferCredits { get; set; }

    public DateOnly? TransferLastSemEnd { get; set; }

    public decimal? TransferGpa { get; set; }

    public long? FinancialNeedStatusId { get; set; }

    public long? SelfRelianceLevelId { get; set; }

    public long? MotivationLevelId { get; set; }

    public long? FutureGoalsLevelId { get; set; }

    public string? RecommendationLetterPath { get; set; }

    public string? RecommendationLetterNotes { get; set; }

    public bool? IsDraft { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string StudentCode { get; set; } = null!;

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ZzMasterDropdown? FinancialNeedStatus { get; set; }

    public virtual ZzMasterDropdown? FutureGoalsLevel { get; set; }

    public virtual ZzMasterDropdown? Gender { get; set; }

    public virtual ICollection<KfStudentAcademicRegistration> KfStudentAcademicRegistrations { get; set; } = new List<KfStudentAcademicRegistration>();

    public virtual ICollection<KfStudentHistory> KfStudentHistories { get; set; } = new List<KfStudentHistory>();

    public virtual ICollection<KfStudentProgramApplication> KfStudentProgramApplications { get; set; } = new List<KfStudentProgramApplication>();

    public virtual ZzMasterDropdown? MotivationLevel { get; set; }

    public virtual ZzMasterCountry? Nationality { get; set; }

    public virtual ZzMasterDropdown? Religion { get; set; }

    public virtual ZzMasterCountry? ResidenceCountry { get; set; }

    public virtual KfSchool School { get; set; } = null!;

    public virtual ZzMasterDropdown? SelfRelianceLevel { get; set; }

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
