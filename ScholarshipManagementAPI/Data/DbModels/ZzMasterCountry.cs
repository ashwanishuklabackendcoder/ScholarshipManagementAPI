using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterCountry
{
    public long CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public int CountryIsdCode { get; set; }

    public string? CountryAlphaCode3 { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfSchool> KfSchools { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfStaff> KfStaffs { get; set; } = new List<KfStaff>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationNationalities { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationResidenceCountries { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfUniversity> KfUniversities { get; set; } = new List<KfUniversity>();

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }

    public virtual ICollection<ZzMasterCurrency> ZzMasterCurrencies { get; set; } = new List<ZzMasterCurrency>();
}
