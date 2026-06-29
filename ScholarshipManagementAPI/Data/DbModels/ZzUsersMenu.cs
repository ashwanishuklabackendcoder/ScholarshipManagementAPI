using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzUsersMenu
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

    public bool? IsDashboard { get; set; }

    public bool ShowInMenu { get; set; }

    public bool? IsApp { get; set; }

    public virtual ICollection<ZzUsersMenu> InverseParent { get; set; } = new List<ZzUsersMenu>();

    public virtual ZzUsersModule Module { get; set; } = null!;

    public virtual ZzUsersMenu? Parent { get; set; }

    public virtual ICollection<ZzUsersRolePage> ZzUsersRolePages { get; set; } = new List<ZzUsersRolePage>();

    public virtual ICollection<ZzUsersRole> ZzUsersRoles { get; set; } = new List<ZzUsersRole>();
}
