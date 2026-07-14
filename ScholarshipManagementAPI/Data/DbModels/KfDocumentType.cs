using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfDocumentType
{
    public long DocumentTypeId { get; set; }

    public string DocumentName { get; set; } = null!;

    public bool IsDefault { get; set; }

    public bool DefaultRequired { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDraft { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfProgramDocument> KfProgramDocuments { get; set; } = new List<KfProgramDocument>();

    public virtual ICollection<StudentProgramDocument> StudentProgramDocuments { get; set; } = new List<StudentProgramDocument>();

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
