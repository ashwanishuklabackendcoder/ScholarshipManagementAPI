using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.DTOs.School.StudentProgramApplication
{
    public class ApplyRequestDto
    {
        public long ProgramId { get; set; }
        public string? Remarks { get; set; }

    }

    public class StudentProgramApplicationResponseDto
    {
        public long ApplicationId { get; set; }
        public long StudentId { get; set; }

        public long ProgramId { get; set; }
        public string ProgramName { get; set; } = null!;
        public string ProgramCode { get; set; } = null!;

        public int ApplicationStatus { get; set; }
        public string ApplicationStatusName { get; set; } = null!;

        public DateTime AppliedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }

        public string? Remarks { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsAllRequiredDocumentsUploaded { get; set; }

        public List<RequiredDocumentDto> RequiredDocuments { get; set; } = new();
        public List<StudentProgramDocumentResponseDto> Documents { get; set; } = new();

        public string UniversityName { get; set; } = null!;
        public string FacultyName { get; set; } = null!;

    }

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

    public class CandidateProgramResponseDto
    {
        public long ProgramId { get; set; }
        public string ProgramName { get; set; } = null!;
        public string ProgramCode { get; set; } = null!;
        public string UniversityName { get; set; } = null!;
        public string FacultyName { get; set; } = null!;
        public List<RequiredDocumentDto> RequiredDocuments { get; set; } = new();
    }

    public class RequiredDocumentDto
    {
        public long ProgramDocumentId { get; set; }
        public long DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; } = null!;
        public bool IsRequired { get; set; }
        public string? Description { get; set; }
    }

    public class UploadDocumentRequestDto
    {
        public long ProgramDocumentId { get; set; }

        public long DocumentTypeId { get; set; }

        public IFormFile File { get; set; } = default!;
    }
}
