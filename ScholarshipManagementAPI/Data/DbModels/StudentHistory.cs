using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentHistory
{
    public long StudentHistoryId { get; set; }

    public long StudentId { get; set; }

    public long? ApplicationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int HistoryType { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual StudentProgramApplication? Application { get; set; }

    public virtual StudentRegistration Student { get; set; } = null!;
}
