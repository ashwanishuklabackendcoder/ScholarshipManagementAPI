using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersRole
{
    public long RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long ModuleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public long? DashboardMenuLinkId { get; set; }

    public virtual UsersMenu? DashboardMenuLink { get; set; }

    public virtual UsersModule Module { get; set; } = null!;

    public virtual ICollection<UsersLoginRole> UsersLoginRoles { get; set; } = new List<UsersLoginRole>();

    public virtual ICollection<UsersRolePage> UsersRolePages { get; set; } = new List<UsersRolePage>();
}
