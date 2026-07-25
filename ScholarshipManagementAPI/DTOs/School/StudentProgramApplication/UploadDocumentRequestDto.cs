namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;

public class UploadDocumentRequestDto
{
    public long ProgramDocumentId { get; set; }

    public long DocumentTypeId { get; set; }

    public IFormFile File { get; set; } = default!;
}
