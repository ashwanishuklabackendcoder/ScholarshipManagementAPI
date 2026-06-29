using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UnUniversityRegistration
{
    public long RegistrationId { get; set; }

    public string UniversityName { get; set; } = null!;

    public string? UniversityType { get; set; }

    public string? CharterAccreditation { get; set; }

    public int? EstablishedYear { get; set; }

    public long CountryId { get; set; }

    public string City { get; set; } = null!;

    public string? Address { get; set; }

    public string? Website { get; set; }

    // Vice Chancellor Details
    public string? VcName { get; set; }

    public string? VcEmail { get; set; }

    public string? VcMobile { get; set; }

    // Coordinator Details
    public string CoordName { get; set; } = null!;

    public string? CoordPosition { get; set; }

    public string CoordEmail { get; set; } = null!;

    public string CoordPhone { get; set; } = null!;

    // Staff Metrics
    public int? FacultiesCount { get; set; }

    public int? FacultyFulltimeCount { get; set; }

    public int? AdminStaffCount { get; set; }

    // Programs Metrics
    public int? ProgDegreeCount { get; set; }

    public int? ProgDiplomaCount { get; set; }

    public int? ProgCertificateCount { get; set; }

    public int? ProgPostgradCount { get; set; }

    // Student Metrics
    public int? StudentsTotal { get; set; }

    public int? StudentsEnrolled { get; set; }

    public decimal? IntlStudentsPct { get; set; }

    public string? StudentsGender { get; set; }

    // Student Breakdown
    public int? StudDegreeCount { get; set; }

    public int? StudDiplomaCount { get; set; }

    public int? StudCertificateCount { get; set; }

    public int? StudPostgradCount { get; set; }

    public int? GraduatesTotal { get; set; }

    public int? AlumniCount { get; set; }

    // Performance Indicators
    public decimal? OpSustainabilityPct { get; set; }

    public decimal? EmployabilityPct { get; set; }

    public decimal? PhdStaffPct { get; set; }

    public string? FteRatio { get; set; }

    public decimal? TeachingLoadHours { get; set; }

    public int? AnnualPublications { get; set; }

    public int? OnlineProgramsCount { get; set; }

    public int? IntlAccreditedProgramsCount { get; set; }

    public string? ExternalGrants { get; set; }

    // Status and Bookkeeping
    public string? Notes { get; set; }

    public int ApprovalStatus { get; set; }

    public long? ApprovedBy { get; set; }

    // Standard Audit Fields
    public bool IsDraft { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    // Navigations
    public virtual ZzMasterCountry Country { get; set; } = null!;

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
