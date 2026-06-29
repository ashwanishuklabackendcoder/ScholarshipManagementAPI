using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfSchool
{
    public long SchoolId { get; set; }

    // Identity
    public string SchoolName { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public byte SchoolType { get; set; }
    public string? OwningInstitution { get; set; }
    public short? SchoolYearOfEstablish { get; set; }

    // Location
    public long CountryId { get; set; }
    public string? Area { get; set; }
    public string? CenterName { get; set; }
    public string? SchoolNumber { get; set; }

    // Academic Setup
    public DateTime? AcademicYearStartDate { get; set; }
    public DateTime? AcademicYearEndDate { get; set; }
    public string? SchoolTeachingLanguage { get; set; }
    public string? SchoolAccreditations { get; set; }
    public bool? IsIslamicCurriculum { get; set; }
    public string? ReligionSubjectCurriculum { get; set; }

    // Capacity & Performance
    public int? TotalStudentsHighSchool { get; set; }
    public int? AverageStudentsPerClass { get; set; }
    public int? SchoolLocalRank { get; set; }
    public bool? IsThreeYearStudentSuccessRateAbove80 { get; set; }
    public bool? IsUniversityEligibilityRateAbove80 { get; set; }
    public bool? IsGraduateEnglishProficiencyAbove80 { get; set; }

    // Contact Information
    public string? SchoolWebsite { get; set; }
    public string? SchoolPhoneNo { get; set; }
    public string? EmailId { get; set; }

    // Principal
    public string? PrincipalName { get; set; }
    public string? PrincipalMobile { get; set; }
    public string? PrincipalEmail { get; set; }

    // Coordinator
    public string? SchoolCoordinatorName { get; set; }
    public string? SchoolCoordinatorMobile { get; set; }
    public string? SchoolCoordinatorEmail { get; set; }

    // System Settings
    public long? DefaultCurrencyId { get; set; }
    public byte SchoolStatus { get; set; }
    public string? StudentCodeFormatPrefix { get; set; }
    public string? StudentCodeFormatSuffix { get; set; }
    public int StudentSequenceNumber { get; set; }

    // Approval Workflow
    public byte AccreditationStatus { get; set; }
    public long? AccreditationBy { get; set; }
    public DateTime? AccreditationDate { get; set; }
    public string? CommitteeComment { get; set; }

    // Audit Fields
    public bool IsDraft { get; set; }
    public bool IsActive { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    // Navigations
    public virtual UsersLogin? AccreditationByNavigation { get; set; }
    public virtual ZzMasterCountry Country { get; set; } = null!;
    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;
    public virtual ZzMasterCurrency? DefaultCurrency { get; set; }
    public virtual ICollection<HrStaffMaster> HrStaffMasters { get; set; } = new List<HrStaffMaster>();
    public virtual ICollection<StudentDatum> StudentData { get; set; } = new List<StudentDatum>();
    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
