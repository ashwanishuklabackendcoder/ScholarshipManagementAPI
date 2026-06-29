using ScholarshipManagementAPI.DTOs.Common.Filter;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.MasterSchool
{
    public class MasterSchoolFilterDto :BaseFilterDto
    {   
        // Search Filters
        
        public string? SchoolName { get; set; }
        public long? CountryId { get; set; }
        public bool? IsActive { get; set; }
        public int? ApprovalStatus { get; set; }

        public string? Area { get; set; }
        public string? CenterName { get; set; }
        public string? SchoolType { get; set; }


        // Date Filters
        public DateTime? AcademicYearStartFrom { get; set; }
        public DateTime? AcademicYearEndTo { get; set; }

    }
}
