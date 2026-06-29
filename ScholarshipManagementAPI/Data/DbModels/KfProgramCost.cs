using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfProgramCost
{
    public long ProgramCostId { get; set; }

    public long ProgramId { get; set; }

    public long SponsorshipTypeId { get; set; }

    public decimal Amount { get; set; }

    public virtual KfProgram Program { get; set; } = null!;

    public virtual KfSponsorshipType SponsorshipType { get; set; } = null!;
}
