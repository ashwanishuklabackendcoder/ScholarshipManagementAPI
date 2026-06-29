namespace ScholarshipManagementAPI.DTOs.School.StudentRequirements
{
    public class UploadDocumentRequestDto
    {
        public Guid UploadSessionId { get; set; }   // required for temp tracking

        public long? StudentReqId { get; set; }     // nullable (before save)

        public long StudentId { get; set; }         // required always

        public long MasterDocId { get; set; }       // required always

        public IFormFile File { get; set; }         // required always
    }
}
