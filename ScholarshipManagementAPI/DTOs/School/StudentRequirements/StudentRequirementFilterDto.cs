using ScholarshipManagementAPI.DTOs.Common.Filter;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.StudentRequirements
{
    public class StudentRequirementFilterDto : BaseFilterDto
    {
        public long? StudentID { get; set; }

        public long? SchoolId { get; set; }

        public string? StudentNumber { get; set; }

        public long? ReqId { get; set; }



        public int? DocumentStatus { get; set; }
        public int? DaAdmissionStatus { get; set; }
        public int? UniAwardingStatus { get; set; }


        public long? DonorId { get; set; }



        // DATE FILTERS
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public DateTime? SemesterStartFrom { get; set; }
        public DateTime? SemesterStartTo { get; set; }


        // COST FILTER
        [Range(0, double.MaxValue)]
        public double? MinTotalCost { get; set; }


        [Range(0, double.MaxValue)]
        public double? MaxTotalCost { get; set; }



    }
}
