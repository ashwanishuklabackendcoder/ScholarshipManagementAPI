using System;

namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;

public class StudentHistoryResponseDto
{
    public long StudentHistoryId { get; set; }
    public long StudentId { get; set; }
    public long? ApplicationId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int HistoryType { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
