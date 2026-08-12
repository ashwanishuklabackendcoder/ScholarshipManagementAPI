using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterCurrency
{
    public long CurrencyId { get; set; }

    public string CurrencyName { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;

    public string CurrencySymbol { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public long CountryId { get; set; }

    public virtual ZzMasterCountry Country { get; set; } = null!;

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfSchool> KfSchools { get; set; } = new List<KfSchool>();

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }

    public virtual ICollection<ZzCurrencyConversion> ZzCurrencyConversions { get; set; } = new List<ZzCurrencyConversion>();
}
