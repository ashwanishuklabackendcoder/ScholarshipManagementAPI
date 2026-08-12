using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzGeneralSetting
{
    public long ConfigId { get; set; }

    public string ConfigKey { get; set; } = null!;

    public string ConfigValue { get; set; } = null!;

    public string? ConfigDescription { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
