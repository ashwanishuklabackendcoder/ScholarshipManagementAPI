namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;

public class RequiredDocumentDto
{
    public long ProgramDocumentId { get; set; }
    public long DocumentTypeId { get; set; }
    public string DocumentTypeName { get; set; } = null!;
    public bool IsRequired { get; set; }
    public string? Description { get; set; }
}
