using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class AdminEmailTemplate
{
    public long EmailTempId { get; set; }

    public long? SchoolId { get; set; }

    public bool IsActive { get; set; }

    public string TemplateName { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? Template { get; set; }

    public bool IsDraft { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
