using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersModule
{
    public long ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin? CreatedByNavigation { get; set; }

    public virtual ICollection<KfStaff> KfStaffs { get; set; } = new List<KfStaff>();

    public virtual ICollection<KfUsersMenu> KfUsersMenus { get; set; } = new List<KfUsersMenu>();

    public virtual UsersLogin? UpdatedByNavigation { get; set; }

    public virtual ICollection<UsersRole> UsersRoles { get; set; } = new List<UsersRole>();

    public virtual ICollection<ZzMasterDropdown> ZzMasterDropdowns { get; set; } = new List<ZzMasterDropdown>();
}
