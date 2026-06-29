using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class AcCurrencyConversion
{
    public long CurrencyConversionId { get; set; }

    public DateOnly FromDate { get; set; }

    public long CurrencyId { get; set; }

    public decimal Rates { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Remarks { get; set; }

    public virtual ZzMasterCurrency Currency { get; set; } = null!;
}
