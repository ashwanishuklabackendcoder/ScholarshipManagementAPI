using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzUsersRole
{
    public long RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long ModuleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public long? DashboardMenuLinkId { get; set; }

    public virtual ZzUsersMenu? DashboardMenuLink { get; set; }

    public virtual ZzUsersModule Module { get; set; } = null!;

    public virtual ICollection<ZzUsersLoginRole> ZzUsersLoginRoles { get; set; } = new List<ZzUsersLoginRole>();

    public virtual ICollection<ZzUsersRolePage> ZzUsersRolePages { get; set; } = new List<ZzUsersRolePage>();
}
