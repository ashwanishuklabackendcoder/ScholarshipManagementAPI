using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersModule
{
    public long ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<HrStaffMaster> HrStaffMasters { get; set; } = new List<HrStaffMaster>();

    public virtual ICollection<UsersMenu> UsersMenus { get; set; } = new List<UsersMenu>();

    public virtual ICollection<UsersRole> UsersRoles { get; set; } = new List<UsersRole>();

    public virtual ICollection<ZzMasterDropDown> ZzMasterDropDowns { get; set; } = new List<ZzMasterDropDown>();
}
