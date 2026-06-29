namespace ScholarshipManagementAPI.DTOs.School.StudentRequirements
{
    public class StudentDocumentDto
    {
        public long DocId { get; set; }
        public long? StdReqId { get; set; }
        public long MasterDocId { get; set; }
        public string DocName { get; set; } = string.Empty;
        public string DocType { get; set; } = string.Empty;
        public bool IsUploaded { get; set; }
        public string? FileUrl { get; set; }


        public DateTime? UploadedAt { get; set; }
    }
}
