using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterDropDown
{
    public long UniqueId { get; set; }

    public string DisplayText { get; set; } = null!;

    public long? ParentId { get; set; }

    public int DisplaySequence { get; set; }

    public bool IsEditable { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsShow { get; set; }

    public string? CreatedBy { get; set; }

    public long? ModuleId { get; set; }

    public bool IsDraft { get; set; }

    public bool IsActive { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<ZzMasterDropDown> InverseParent { get; set; } = new List<ZzMasterDropDown>();

    public virtual ICollection<KfSchool> KfSchoolSchoolStatusNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSchool> KfSchoolSchoolTypeNavigations { get; set; } = new List<KfSchool>();

    public virtual UsersModule? Module { get; set; }

    public virtual ZzMasterDropDown? Parent { get; set; }

    public virtual ICollection<StudentRegistration> StudentRegistrationFinancialNeedStatuses { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationFutureGoalsLevels { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationGenders { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationMotivationLevels { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationReligions { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationSelfRelianceLevels { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationStudentsGenderTypes { get; set; } = new List<UnUniversityRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationUniversityTypeNavigations { get; set; } = new List<UnUniversityRegistration>();
}
