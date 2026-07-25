namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;

/// <summary>
/// manange by schools
/// </summary>

public class ApplyRequestDto
{
    public long ProgramId { get; set; }
    public string? Remarks { get; set; }

}
