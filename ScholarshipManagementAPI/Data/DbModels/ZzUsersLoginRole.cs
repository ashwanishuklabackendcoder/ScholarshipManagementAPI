using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzUsersLoginRole
{
    public long UserLoginRoleId { get; set; }

    public long RoleId { get; set; }

    public long LoginId { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual ZzUsersLogin Login { get; set; } = null!;

    public virtual ZzUsersRole Role { get; set; } = null!;
}
