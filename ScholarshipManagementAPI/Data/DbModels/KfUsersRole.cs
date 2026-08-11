using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfUsersRole
{
    public long RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long ModuleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfUsersLoginRolesAssignment> KfUsersLoginRolesAssignments { get; set; } = new List<KfUsersLoginRolesAssignment>();

    public virtual ICollection<KfUsersRolePermission> KfUsersRolePermissions { get; set; } = new List<KfUsersRolePermission>();

    public virtual KfUsersModule Module { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
