using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzUsersLogin
{
    public long LoginId { get; set; }

    public int? LoginType { get; set; }

    public string LoginName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ForgotEmail { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public long? UniversityId { get; set; }

    public long? SchoolListId { get; set; }

    public string? Language { get; set; }

    public virtual ICollection<ZzUsersLoginRole> ZzUsersLoginRoles { get; set; } = new List<ZzUsersLoginRole>();

    public virtual ICollection<ZzUsersLoginsLog> ZzUsersLoginsLogs { get; set; } = new List<ZzUsersLoginsLog>();
}
