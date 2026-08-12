using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzCurrencyConversion
{
    public long CurrencyConversionId { get; set; }

    public DateOnly FromDate { get; set; }

    public long CurrencyId { get; set; }

    public decimal Rates { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ZzMasterCurrency Currency { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
