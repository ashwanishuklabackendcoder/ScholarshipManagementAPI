using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.StudentRequirements
{
    public class StudentRequirementRequestDto
    {
        public long? StudentReqID { get; set; }   // null / 0 = Create, >0 = Update

        
        public long StudentID { get; set; }

        
        public long ReqId { get; set; }



        /// <summary>
        /// filled by university after receiving the application, 
        /// it will be used to track the status of the application
        /// </summary>

        [Range(0, 10, ErrorMessage = "Invalid Document Status")]
        public int? DocumentStatus { get; set; }

        [StringLength(500, ErrorMessage = "Max 500 characters allowed")]
        public string? ReasonRejection { get; set; }

        [StringLength(500)]
        public string? MissedDocuments { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SemesterStartDate { get; set; }

        [StringLength(200)]
        public string? LetterAccepCode { get; set; }

        public long? UniStatusBy { get; set; }

        public DateTime? UniStatusDate { get; set; }


        /// <summary>
        /// 
        /// </summary>

        [Range(0, 10)]
        public int? DaAdmissionStatus { get; set; }

        public long? DaStatusBy { get; set; }

        public DateTime? DaStatusDate { get; set; }

        public long? DonorId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total cost must be positive")]
        public double? TotalCost { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(200)]
        public string? CreateEmailBy { get; set; }

        [StringLength(500)]
        public string? ReasonInProgress { get; set; }

        [Range(0, 10)]
        public int? UniAwardingStatus { get; set; }

        [Range(0, double.MaxValue)]
        public double? UniAwardingStatusCost { get; set; }




        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }


        // for response
        public string? AcademicYear { get; set; }
        public string? StudentFullName { get; set; }
        public string? StudentNumber { get; set; }
        public string? StudentPhoto { get; set; }
        public string? CourseName { get; set; }
        public long? CourseId { get; set; }
        public string? CourseTypeName { get; set; }
        public long? CourseTypeId { get; set; }
        public string? UniversityName { get; set; }
        public long? UniversityId { get; set; }


        public string? RequiredDocuments { get; set; }

        public string? DonorName { get; set; }

    }
}
