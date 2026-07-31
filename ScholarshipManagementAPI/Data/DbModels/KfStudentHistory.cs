using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfStudentHistory
{
    public long StudentHistoryId { get; set; }

    public long StudentId { get; set; }

    public long? ApplicationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int HistoryType { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual KfStudentProgramApplication? Application { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfStudentRegistration Student { get; set; } = null!;
}
