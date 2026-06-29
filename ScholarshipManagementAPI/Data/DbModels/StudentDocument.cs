using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentDocument
{
    public long DocumentId { get; set; }

    public long StudentId { get; set; }

    public string? DocType { get; set; }

    public string? DocName { get; set; }

    public string? FileUrlName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? StudentReqId { get; set; }

    public long MasterDocId { get; set; }

    public Guid? UploadSessionId { get; set; }

    public virtual UnMasterDoc MasterDoc { get; set; } = null!;

    public virtual StudentDatum Student { get; set; } = null!;

    public virtual StudentReqList? StudentReq { get; set; }
}
