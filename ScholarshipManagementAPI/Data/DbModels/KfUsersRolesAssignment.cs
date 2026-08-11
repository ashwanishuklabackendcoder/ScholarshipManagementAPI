using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfUsersRolesAssignment
{
    public long UserLoginRoleId { get; set; }

    public long RoleId { get; set; }

    public long LoginId { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual UsersLogin Login { get; set; } = null!;

    public virtual KfUsersRole Role { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
