using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzUsersRolePage
{
    public long RoleFormId { get; set; }

    public long RoleId { get; set; }

    public long MenuLinkId { get; set; }

    public bool InsertPer { get; set; }

    public bool UpdatePer { get; set; }

    public bool DeletePer { get; set; }

    public bool ViewPer { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ZzUsersMenu MenuLink { get; set; } = null!;

    public virtual ZzUsersRole Role { get; set; } = null!;
}
