using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementAPI.DTOs.School.StudentRequirements
{
    public class DocumentStatusRequestDto
    {
        public long? StudentReqId { get; set; }   // After save

        public Guid? UploadSessionId { get; set; } // Before save


        [Required(ErrorMessage = "Requirement ID is required")]
        public long ReqId { get; set; }           // Required always
    }
}
