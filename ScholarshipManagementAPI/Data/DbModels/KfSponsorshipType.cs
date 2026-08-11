using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfSponsorshipType
{
    public long SponsorshipTypeId { get; set; }

    public string SponsorshipName { get; set; } = null!;

    public byte FrequencyType { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfProgramCost> KfProgramCosts { get; set; } = new List<KfProgramCost>();

    public virtual ICollection<KfSponsorshipCategoryMapping> KfSponsorshipCategoryMappings { get; set; } = new List<KfSponsorshipCategoryMapping>();

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
