using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.MasterSchool
{
    public class MasterSchoolRequestDto
    {
        public long? SchoolId { get; set; } // null/0 = Create, >0 = Update

        #region Identity

        [Required]
        [MaxLength(300)]
        public string SchoolName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; } = string.Empty;

        [Required]
        public byte SchoolType { get; set; }

        [MaxLength(300)]
        public string? OwningInstitution { get; set; }

        public short? SchoolYearOfEstablish { get; set; }

        #endregion

        #region Location

        [Required]
        public long CountryId { get; set; }

        [MaxLength(200)]
        public string? Area { get; set; }

        [MaxLength(200)]
        public string? CenterName { get; set; }

        [MaxLength(100)]
        public string? SchoolNumber { get; set; }

        #endregion

        #region Academic

        public DateTime? AcademicYearStartDate { get; set; }

        public DateTime? AcademicYearEndDate { get; set; }

        public string? SchoolTeachingLanguage { get; set; }

        public string? SchoolAccreditations { get; set; }

        public bool? IsIslamicCurriculum { get; set; }

        [MaxLength(500)]
        public string? ReligionSubjectCurriculum { get; set; }

        #endregion

        #region Capacity & Performance

        public int? TotalStudentsHighSchool { get; set; }

        public int? AverageStudentsPerClass { get; set; }

        public int? SchoolLocalRank { get; set; }

        public bool? IsThreeYearStudentSuccessRateAbove80 { get; set; }

        public bool? IsUniversityEligibilityRateAbove80 { get; set; }

        public bool? IsGraduateEnglishProficiencyAbove80 { get; set; }

        #endregion

        #region Contact

        [MaxLength(500)]
        public string? SchoolWebsite { get; set; }

        [MaxLength(50)]
        public string? SchoolPhoneNo { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? EmailId { get; set; }

        #endregion

        #region Principal

        [MaxLength(200)]
        public string? PrincipalName { get; set; }

        [MaxLength(50)]
        public string? PrincipalMobile { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? PrincipalEmail { get; set; }

        #endregion

        #region Coordinator

        [MaxLength(200)]
        public string? SchoolCoordinatorName { get; set; }

        [MaxLength(50)]
        public string? SchoolCoordinatorMobile { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? SchoolCoordinatorEmail { get; set; }

        #endregion

        #region System Settings

        public long? DefaultCurrencyId { get; set; }

        public byte SchoolStatus { get; set; } = 1;

        [MaxLength(20)]
        public string? StudentCodeFormatPrefix { get; set; }

        [MaxLength(20)]
        public string? StudentCodeFormatSuffix { get; set; }

        public int StudentSequenceNumber { get; set; } = 1;

        #endregion

        #region Accreditation

        public byte AccreditationStatus { get; set; } = 1;

        public long? AccreditationBy { get; set; }

        public DateTime? AccreditationDate { get; set; }

        [MaxLength(2000)]
        public string? CommitteeComment { get; set; }

        #endregion

        #region Audit

        public bool IsDraft { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        #endregion

        #region Response Only

        public string? CountryName { get; set; }

        public string? AccreditationByName { get; set; }

        public string? DefaultCurrencyName { get; set; }

        public int TotalStudents { get; set; }

        #endregion
    }
}
