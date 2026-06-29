using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UnUniversityList
{
    public long UniversityId { get; set; }

    public string UniversityName { get; set; } = null!;

    public bool IsActive { get; set; }

    public long CountryId { get; set; }

    public bool IsApproved { get; set; }

    public long? ApprovedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Remarks { get; set; }

    public long? DefaultCurrencyId { get; set; }

    public virtual HrStaffMaster? ApprovedByNavigation { get; set; }

    public virtual ZzMasterCountry Country { get; set; } = null!;

    public virtual ZzMasterCurrency? DefaultCurrency { get; set; }

    public virtual ICollection<HrStaffMaster> HrStaffMasters { get; set; } = new List<HrStaffMaster>();

    public virtual ICollection<KfCourse> KfCourses { get; set; } = new List<KfCourse>();

    public virtual ICollection<KfFaculty> KfFaculties { get; set; } = new List<KfFaculty>();

    public virtual ICollection<KfProgram> KfPrograms { get; set; } = new List<KfProgram>();

    public virtual ICollection<UnMasterCourseType> UnMasterCourseTypes { get; set; } = new List<UnMasterCourseType>();

    public virtual ICollection<UnMasterCourse> UnMasterCourses { get; set; } = new List<UnMasterCourse>();

    public virtual ICollection<UnMasterDoc> UnMasterDocs { get; set; } = new List<UnMasterDoc>();
}
