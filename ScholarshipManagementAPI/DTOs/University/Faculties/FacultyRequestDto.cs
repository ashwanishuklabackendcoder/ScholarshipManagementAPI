using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.Faculties
{
    public class FacultyRequestDto
    {
        public long? FacultyId { get; set; }                                    // null for create, value for update

        [Required]
        public long UniversityId { get; set; }

        [Required]
        [StringLength(200)]
        public string FacultyName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FacultyCode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;



        // for Response only
        public string? UniversityName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public string? CreatedByName { get; set; } 

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }

    }
}
