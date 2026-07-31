using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfStudentAcademicRegistration
{
    public long Id { get; set; }

    public long StudentId { get; set; }

    public long ApplicationId { get; set; }

    public long ProgramId { get; set; }

    public int SemesterNo { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? Remarks { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual KfStudentProgramApplication Application { get; set; } = null!;

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfProgram Program { get; set; } = null!;

    public virtual KfStudentRegistration Student { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
