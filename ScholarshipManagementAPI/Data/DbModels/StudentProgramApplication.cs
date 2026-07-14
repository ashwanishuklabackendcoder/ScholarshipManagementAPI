using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentProgramApplication
{
    public long ApplicationId { get; set; }

    public long StudentId { get; set; }

    public long ProgramId { get; set; }

    public int ApplicationStatus { get; set; }

    public DateTime AppliedDate { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public string? Remarks { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfProgram Program { get; set; } = null!;

    public virtual StudentRegistration Student { get; set; } = null!;

    public virtual ICollection<StudentHistory> StudentHistories { get; set; } = new List<StudentHistory>();

    public virtual ICollection<StudentProgramDocument> StudentProgramDocuments { get; set; } = new List<StudentProgramDocument>();
}
