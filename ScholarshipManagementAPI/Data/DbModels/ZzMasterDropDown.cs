using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterDropDown
{
    public long UniqueId { get; set; }

    public string DisplayText { get; set; } = null!;

    public long? ParentId { get; set; }

    public int DisplaySequence { get; set; }

    public bool Status { get; set; }

    public bool IsEditable { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsShow { get; set; }

    public string? CreatedBy { get; set; }

    public long? ModuleId { get; set; }

    public virtual ICollection<ZzMasterDropDown> InverseParent { get; set; } = new List<ZzMasterDropDown>();

    public virtual UsersModule? Module { get; set; }

    public virtual ZzMasterDropDown? Parent { get; set; }
}
