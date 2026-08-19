using ScholarshipManagementAPI.DTOs.Common.Filter;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.MasterSchool
{
    public class MasterSchoolFilterDto :BaseFilterDto
    {   
        // Search Filters
        
        public long? CountryId { get; set; }
        public long? SchoolType { get; set; }
        public long? SchoolStatus { get; set; }

        public byte? AccreditationStatus { get; set; }

        public bool? IsActive { get; set; }

        public bool? MySchools { get; set; }

    }
}
