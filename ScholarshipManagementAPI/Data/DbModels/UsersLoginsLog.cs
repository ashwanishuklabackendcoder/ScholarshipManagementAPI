using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersLoginsLog
{
    public long LoginLogId { get; set; }

    public long LoginId { get; set; }

    public string IpAddress { get; set; } = null!;

    public DateTime LoginDateTime { get; set; }

    public DateTime? LogoutDateTime { get; set; }

    public string? BrowserName { get; set; }

    public string? OperatingSystem { get; set; }

    public string? ComputerName { get; set; }

    public string? UserName { get; set; }

    public virtual UsersLogin Login { get; set; } = null!;
}
