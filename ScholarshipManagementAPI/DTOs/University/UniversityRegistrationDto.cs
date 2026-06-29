using System;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University
{
    public class UniversityRegistrationDto
    {
        public long? RegistrationId { get; set; }

        [Required]
        [MaxLength(300)]
        public string UniversityName { get; set; } = null!;

        [MaxLength(100)]
        public string? UniversityType { get; set; }

        [MaxLength(500)]
        public string? CharterAccreditation { get; set; }

        public int? EstablishedYear { get; set; }

        [Required]
        public long CountryId { get; set; }

        [Required]
        [MaxLength(150)]
        public string City { get; set; } = null!;

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(250)]
        public string? Website { get; set; }

        // Vice Chancellor Details
        [MaxLength(200)]
        public string? VcName { get; set; }

        [MaxLength(150)]
        [EmailAddress]
        public string? VcEmail { get; set; }

        [MaxLength(50)]
        public string? VcMobile { get; set; }

        // Coordinator Details
        [Required]
        [MaxLength(200)]
        public string CoordName { get; set; } = null!;

        [MaxLength(150)]
        public string? CoordPosition { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string CoordEmail { get; set; } = null!;

        [Required]
        [MaxLength(50)]
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

        // Audit & Visual Info Response
        public string? CountryName { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
