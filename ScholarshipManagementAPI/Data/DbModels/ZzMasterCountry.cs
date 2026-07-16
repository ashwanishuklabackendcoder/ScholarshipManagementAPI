using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterCountry
{
    public long CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public int CountryIsdCode { get; set; }

    public string? CountryAlphaCode3 { get; set; }

    public string? CurrencyName { get; set; }

    public string? CurrencyFracUnit { get; set; }

    public string? CurrencySymbol { get; set; }

    public string? CurrencyAbb { get; set; }

    public bool IsActive { get; set; }

    public bool IsDraft { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<KfSchool> KfSchools { get; set; } = new List<KfSchool>();

    public virtual ICollection<StudentRegistration> StudentRegistrationNationalities { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationResidenceCountries { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrations { get; set; } = new List<UnUniversityRegistration>();
}
