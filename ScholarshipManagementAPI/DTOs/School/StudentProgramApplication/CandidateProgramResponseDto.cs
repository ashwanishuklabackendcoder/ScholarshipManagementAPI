using System.Collections.Generic;

namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;

public class CandidateProgramResponseDto
{
    public long ProgramId { get; set; }
    public string ProgramName { get; set; } = null!;
    public string ProgramCode { get; set; } = null!;
    public string UniversityName { get; set; } = null!;
    public string FacultyName { get; set; } = null!;



    // NEW -- if any candidate applied to any program , then this will be filled with the application id and status
    public long? ApplicationId { get; set; }
    public int? ApplicationStatus { get; set; }
    public string? ApplicationStatusName { get; set; }

    public List<RequiredDocumentDto> RequiredDocuments { get; set; } = new();
}
