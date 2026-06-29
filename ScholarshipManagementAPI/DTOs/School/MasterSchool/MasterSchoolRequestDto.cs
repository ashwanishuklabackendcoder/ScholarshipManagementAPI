using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.MasterSchool
{
    public class MasterSchoolRequestDto
    {
        public long? SchoolId { get; set; }   // null / 0 = Create, >0 = Update

        [Required]
        [MaxLength(500)]
        public string SchoolName { get; set; } = null!;

        [Required]
        public long CountryId { get; set; }

        [MaxLength(200)]
        public string? StudentCodeFormatPrefix { get; set; }

        [MaxLength(200)]
        public string? StudentCodeFormatSufix { get; set; }

        [MaxLength(200)]
        public string? StudentCodeFormatStartingNo { get; set; }

        [MaxLength(200)]
        public string? StudentCodeFormatLastSavedNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Area { get; set; }

        [MaxLength(200)]
        public string? CenterName { get; set; }

        [MaxLength(200)]
        public string? SchoolNumber { get; set; }

        [MaxLength(200)]
        public string? SchoolYearOfEstablish { get; set; }

        [MaxLength(200)]
        public string? SchoolType { get; set; }

        [MaxLength(200)]
        public string? SchoolTeachingLanguage { get; set; }

        [MaxLength(200)]
        public string? GraduatesEnglishLessThan { get; set; }

        [MaxLength(200)]
        public string? TotalNumberOfHighSchoolLevel { get; set; }

        [MaxLength(200)]
        public string? AverageNumberOfStudentPerClass { get; set; }

        [MaxLength(200)]
        public string? SchoolAccreditations { get; set; }

        [MaxLength(200)]
        public string? SchoolSubjectCurriculum { get; set; }

        [MaxLength(200)]
        public string? StudentSuccessAverage { get; set; }

        [MaxLength(200)]
        public string? AverageSchoolGraduates { get; set; }

        [MaxLength(200)]
        public string? SchoolLocalRank { get; set; }

        [MaxLength(200)]
        public string? OwningInstitution { get; set; }

        [MaxLength(200)]
        public string? SchoolWebsite { get; set; }

        [MaxLength(200)]
        public string? SchoolPhoneNo { get; set; }

        public bool IsActive { get; set; }


        [EmailAddress]
        public string? EmailId { get; set; }

        [MaxLength(200)]
        public string? SchoolCoordinatorName { get; set; }

        [MaxLength(200)]
        public string? SchoolCoordinatorMobile { get; set; }

        [EmailAddress]
        public string? SchoolCoordinatorEmail { get; set; }

        public DateTime? AcademicYearStartDate { get; set; }
        public DateTime? AcademicYearEndDate { get; set; }


        public int ApprovalStatus { get; set; }
        public long? ApprovedBy { get; set; }

        public long? DefaultCurrencyId { get; set; }

        // For response only
        public string? CountryName { get; set; }
        public string? ApprovedByName { get; set; }

        public int TotalStudents { get; set; }
    }
}
