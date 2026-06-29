using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzLabel
{
    public long LabelId { get; set; }

    public string LabelName { get; set; } = null!;

    public string LabelValue { get; set; } = null!;

    public string? LabelNewValue { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Arabic { get; set; }
}
