using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentProgramDocument
{
    public long StudentProgramDocumentId { get; set; }

    public long ApplicationId { get; set; }

    public long ProgramDocumentId { get; set; }

    public long DocumentTypeId { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string StoredFileName { get; set; } = null!;

    public string StoragePath { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    public string? ReviewerRemark { get; set; }

    public long UploadedBy { get; set; }

    public DateTime UploadedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual StudentProgramApplication Application { get; set; } = null!;

    public virtual KfDocumentType DocumentType { get; set; } = null!;

    public virtual KfProgramDocument ProgramDocument { get; set; } = null!;
}
