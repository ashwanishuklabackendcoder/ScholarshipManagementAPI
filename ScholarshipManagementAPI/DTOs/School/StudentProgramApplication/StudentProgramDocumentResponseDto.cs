using System;

namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;

public class StudentProgramDocumentResponseDto
{
    public long StudentProgramDocumentId { get; set; }
    public long ApplicationId { get; set; }
    public long ProgramDocumentId { get; set; }
    public long DocumentTypeId { get; set; }
    public string DocumentTypeName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string StoredFileName { get; set; } = null!;
    public string StoragePath { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long FileSize { get; set; }
    public string? ReviewerRemark { get; set; }
    public long UploadedBy { get; set; }
    public DateTime UploadedDate { get; set; }
    public bool IsRequired { get; set; }
}
