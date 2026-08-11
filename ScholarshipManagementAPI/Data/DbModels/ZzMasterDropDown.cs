using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class ZzMasterDropDown
{
    public long UniqueId { get; set; }

    public string DisplayText { get; set; } = null!;

    public long? ParentId { get; set; }

    public int DisplaySequence { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public long? ModuleId { get; set; }

    public bool IsDraft { get; set; }

    public bool IsActive { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ZzMasterDropDown> InverseParent { get; set; } = new List<ZzMasterDropDown>();

    public virtual ICollection<KfSchool> KfSchoolSchoolStatusNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSchool> KfSchoolSchoolTypeNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfStaff> KfStaffs { get; set; } = new List<KfStaff>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationFinancialNeedStatuses { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationFutureGoalsLevels { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationGenders { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationMotivationLevels { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationReligions { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationSelfRelianceLevels { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfUniversity> KfUniversityStudentsGenderTypes { get; set; } = new List<KfUniversity>();

    public virtual ICollection<KfUniversity> KfUniversityUniversityTypeNavigations { get; set; } = new List<KfUniversity>();

    public virtual KfUsersModule? Module { get; set; }

    public virtual ZzMasterDropDown? Parent { get; set; }

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
