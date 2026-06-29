using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfProgramDocument
{
    public long ProgramDocumentId { get; set; }

    public long ProgramId { get; set; }

    public long DocumentTypeId { get; set; }

    public bool IsRequired { get; set; }

    public int? DisplayOrder { get; set; }

    public virtual KfDocumentType DocumentType { get; set; } = null!;

    public virtual KfProgram Program { get; set; } = null!;
}
