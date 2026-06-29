using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.MasterCourseType
{
    public class CourseTypeRequestDto
    {

        public long? CourseTypeId { get; set; } // null for create, value for update


        [StringLength(200)]
        public string CourseTypeName { get; set; } = string.Empty;
        public long UniversityId { get; set; }


        public int ApprovalStatus { get; set; }
        public long? ApprovedBy { get; set; }
        public bool IsActive { get; set; }

        
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;


        // for response
        public string? UniversityName { get; set; }
        public string? ApprovedByName { get; set; }
    }
}
