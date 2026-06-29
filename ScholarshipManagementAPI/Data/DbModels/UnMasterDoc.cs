using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UnMasterDoc
{
    public long UniversityDocsId { get; set; }

    public long UniversityId { get; set; }

    public bool IsDownloadable { get; set; }

    public string DocumentName { get; set; } = null!;

    public string? FileName { get; set; }

    public string? DocType { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<StudentDocument> StudentDocuments { get; set; } = new List<StudentDocument>();

    public virtual UnUniversityList University { get; set; } = null!;
}
