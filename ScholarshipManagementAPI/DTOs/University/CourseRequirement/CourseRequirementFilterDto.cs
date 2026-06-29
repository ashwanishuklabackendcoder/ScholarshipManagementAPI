using ScholarshipManagementAPI.DTOs.Common.Filter;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.CourseRequirement
{
    public class CourseRequirementFilterDto : BaseFilterDto
    {
        public long? CourseId { get; set; }

        public string? AcademicYear { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? ReqStartDate { get; set; }
        public DateTime? ReqEndDate { get; set; }

        public int? ApprovalStatus { get; set; }

        public long? UniversityId { get; set; }
    }
}
