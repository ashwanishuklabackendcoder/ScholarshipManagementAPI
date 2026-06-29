using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.Common.HrStaff
{
    public class StaffRequestDto
    {
        public long? StaffId { get; set; }   // null / 0 = Create, >0 = Update


        // ===== Staff Type & Organisation =====
        [Required]
        public long StaffType { get; set; }          // UsersModule.ModuleId

        public long? OrganisationId { get; set; }    // Required for School / University

        // explicit organisation mapping
        public long? UniversityId { get; set; }
        public long? SchoolId { get; set; }
        public long? NgoId { get; set; }


        // ===== Personal Info =====
        [Required, StringLength(100)]
        public string StaffSalutation { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string StaffFirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string StaffLastName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Gender { get; set; } = string.Empty;

        // ===== Address =====
        [StringLength(200)]
        public string? PermAddress { get; set; }

        [StringLength(100)]
        public string? PermCity { get; set; }

        [StringLength(50)]
        public string? PermZipCode { get; set; }

        [StringLength(100)]
        public string? PermState { get; set; }

        [StringLength(100)]
        public string? PermCountry { get; set; }

        // ===== Contact =====
        [Required, StringLength(100)]
        public string OfficeEmail { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string? PersonelEmail { get; set; }

        [StringLength(100)]
        public string? MobileNo { get; set; }

        // ===== Login Info =====
        [StringLength(200)]
        public string? LoginName { get; set; }

        // ===== Other =====
        [StringLength(200)]
        public string? Photo { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public string? Language { get; set; }

        public bool IsActive { get; set; }

        // ===== Audit (Response mostly) =====
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        // ===== Extra (Response convenience) =====
        public string? StaffTypeName { get; set; }      // From UsersModule
        public string? OrganisationName { get; set; }   // School / University name
    }
}
