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

    public long? FromDaSchool { get; set; }

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

    public DateOnly? TransferLastSemEnd { get; set; }

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

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfSchool? FromDaSchoolNavigation { get; set; }

    public virtual ZzMasterCountry? NationalityNavigation { get; set; }

    public virtual ZzMasterCountry? ResidenceCountryNavigation { get; set; }

    public virtual ICollection<StudentHistory> StudentHistories { get; set; } = new List<StudentHistory>();

    public virtual ICollection<StudentProgramApplication> StudentProgramApplications { get; set; } = new List<StudentProgramApplication>();

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
