using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.Courses
{
    public class CourseRequestDto
    {
        public long? CourseId { get; set; }                               // For Update scenarios - Optional for Create, Required for Update

        [Required]
        public long UniversityId { get; set; }

        [Required]
        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string CourseNameEn { get; set; } = string.Empty;

        [StringLength(300)]
        public string? CourseNameAr { get; set; }

        public bool IsActive { get; set; } = true;



        // Faculties Mapping
        public List<CourseFacultyDto> Faculties { get; set; } = new();
        public List<int> FacultyIds { get; set; } = new();


        // Response
        public string? UniversityName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
    }
}
