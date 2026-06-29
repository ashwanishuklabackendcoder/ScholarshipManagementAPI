using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzUsersModule
{
    public long ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<ZzMasterDropDown> ZzMasterDropDowns { get; set; } = new List<ZzMasterDropDown>();

    public virtual ICollection<ZzUsersMenu> ZzUsersMenus { get; set; } = new List<ZzUsersMenu>();

    public virtual ICollection<ZzUsersRole> ZzUsersRoles { get; set; } = new List<ZzUsersRole>();
}
