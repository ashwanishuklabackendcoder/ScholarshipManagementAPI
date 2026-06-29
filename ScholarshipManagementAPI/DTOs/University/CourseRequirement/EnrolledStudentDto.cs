using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.University.CourseRequirement
{
    public class EnrolledStudentDto
    {
        public long StudentId { get; set; }
        public long ReqId { get; set; }

        public string? StudentFullName { get; set; }
        public string? StudentNumber { get; set; }
        public string? StudentPhoto { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? StudentEmail { get; set; }
        public string? StudentMobileNo { get; set; }


        public string? SchoolName { get; set; }
        public string? ShortName { get; set; }
        public string? SchoolEmail { get; set; }
        public string? SchoolMobileNo { get; set; }
        public string? SchoolWebsite { get; set; }


        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(200)]
        public string? AddressCity { get; set; }

        [StringLength(200)]
        public string? MasterState { get; set; }

        [StringLength(200)]
        public string? MasterCountry { get; set; }

        [StringLength(50)]
        public string? ZipCode { get; set; }


        public string? DocStatus { get; set; }
        public string? AwardingStatus { get; set; }

        public string? SponsoredStatus { get; set; }
        public string? DonorName { get; set; }
        public long? DonorId { get; set; }
        public double? TotalCost { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

