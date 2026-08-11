using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfUsersRoleAssignment
{
    public long UserLoginRoleId { get; set; }

    public long RoleId { get; set; }

    public long LoginId { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfUsersLogin Login { get; set; } = null!;

    public virtual KfUsersRole Role { get; set; } = null!;
}
