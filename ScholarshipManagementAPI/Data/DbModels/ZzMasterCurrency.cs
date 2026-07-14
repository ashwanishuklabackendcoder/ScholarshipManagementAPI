using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterCurrency
{
    public long CurrencyId { get; set; }

    public string CurrencyName { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;

    public string CurrencySymbol { get; set; } = null!;

    public string? CurrencyFracUnit { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsDraft { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<AcCurrencyConversion> AcCurrencyConversions { get; set; } = new List<AcCurrencyConversion>();

    public virtual ICollection<KfSchool> KfSchools { get; set; } = new List<KfSchool>();
}
