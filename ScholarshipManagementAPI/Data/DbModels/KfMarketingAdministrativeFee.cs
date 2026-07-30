using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfMarketingAdministrativeFee
{
    public long MarketingAdministrativeFeeId { get; set; }

    public decimal FeePercentage { get; set; }

    public bool IsCurrent { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
