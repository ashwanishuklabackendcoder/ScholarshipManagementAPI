using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersMenu
{
    public long MenuLinkId { get; set; }

    public long ModuleId { get; set; }

    public string PageHeading { get; set; } = null!;

    public long? ParentId { get; set; }

    public string PagePath { get; set; } = null!;

    public string ActualName { get; set; } = null!;

    public bool IsView { get; set; }

    public int LevelNo { get; set; }

    public int SequenceNo { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool IsDashboard { get; set; }

    public bool ShowInMenu { get; set; }

    public string? Icon { get; set; }

    public virtual ICollection<UsersMenu> InverseParent { get; set; } = new List<UsersMenu>();

    public virtual UsersModule Module { get; set; } = null!;

    public virtual UsersMenu? Parent { get; set; }

    public virtual ICollection<UsersRolePage> UsersRolePages { get; set; } = new List<UsersRolePage>();

    public virtual ICollection<UsersRole> UsersRoles { get; set; } = new List<UsersRole>();
}
