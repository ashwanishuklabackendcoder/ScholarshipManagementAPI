using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfUsersMenu
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

    public long CreatedBy { get; set; }

    public string? Icon { get; set; }

    public bool IsActive { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfUsersMenu> InverseParent { get; set; } = new List<KfUsersMenu>();

    public virtual ICollection<KfUsersRolePermission> KfUsersRolePermissions { get; set; } = new List<KfUsersRolePermission>();

    public virtual KfUsersModule Module { get; set; } = null!;

    public virtual KfUsersMenu? Parent { get; set; }

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
