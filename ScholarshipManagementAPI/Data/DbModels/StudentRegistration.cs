using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentRegistration
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

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ZzMasterDropDown? FinancialNeedStatus { get; set; }

    public virtual ZzMasterDropDown? FutureGoalsLevel { get; set; }

    public virtual ZzMasterDropDown? Gender { get; set; }

    public virtual ZzMasterDropDown? MotivationLevel { get; set; }

    public virtual ZzMasterCountry? Nationality { get; set; }

    public virtual ZzMasterDropDown? Religion { get; set; }

    public virtual ZzMasterCountry? ResidenceCountry { get; set; }

    public virtual KfSchool School { get; set; } = null!;

    public virtual ZzMasterDropDown? SelfRelianceLevel { get; set; }

    public virtual ICollection<StudentHistory> StudentHistories { get; set; } = new List<StudentHistory>();

    public virtual ICollection<StudentProgramApplication> StudentProgramApplications { get; set; } = new List<StudentProgramApplication>();

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
