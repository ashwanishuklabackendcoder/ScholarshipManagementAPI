using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.StudentRequirements
{
    public class StudentRequirementMappingDto
    {
        public long? StudentReqID { get; set; }      // null / 0 = Create, >0 = Update

        [Required(ErrorMessage = "Student is required")]
        public long StudentID { get; set; }

        [Required(ErrorMessage = "Request is required")]
        public long ReqId { get; set; }

        [Required(ErrorMessage = "Univesristy is required")]
        public long UniversityId { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }




        public Guid UploadSessionId { get; set; }   // required for temp tracking of uploaded documents before final save

    }
}
