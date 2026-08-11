using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfProgramRegistrationWindow
{
    public long Id { get; set; }

    public long ProgramId { get; set; }

    public int SemesterNo { get; set; }

    public DateTime RegistrationFrom { get; set; }

    public DateTime RegistrationTo { get; set; }

    public string? Notes { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfProgram Program { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
