using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersLogin
{
    public long LoginId { get; set; }

    public long StaffId { get; set; }

    public string LoginName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ForgotEmail { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Language { get; set; }

    public string? TempPassword { get; set; }

    public DateTime? TempPassDateTime { get; set; }

    public bool IsDraft { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<KfCourse> KfCourseCreatedByNavigations { get; set; } = new List<KfCourse>();

    public virtual ICollection<KfCourse> KfCourseUpdatedByNavigations { get; set; } = new List<KfCourse>();

    public virtual ICollection<KfDocumentType> KfDocumentTypeCreatedByNavigations { get; set; } = new List<KfDocumentType>();

    public virtual ICollection<KfDocumentType> KfDocumentTypeUpdatedByNavigations { get; set; } = new List<KfDocumentType>();

    public virtual ICollection<KfFaculty> KfFacultyCreatedByNavigations { get; set; } = new List<KfFaculty>();

    public virtual ICollection<KfFaculty> KfFacultyUpdatedByNavigations { get; set; } = new List<KfFaculty>();

    public virtual ICollection<KfProgram> KfProgramCreatedByNavigations { get; set; } = new List<KfProgram>();

    public virtual ICollection<KfProgram> KfProgramUpdatedByNavigations { get; set; } = new List<KfProgram>();

    public virtual ICollection<KfSchool> KfSchoolAccreditationByNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSchool> KfSchoolCreatedByNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSchool> KfSchoolUpdatedByNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSponsorshipType> KfSponsorshipTypeCreatedByNavigations { get; set; } = new List<KfSponsorshipType>();

    public virtual ICollection<KfSponsorshipType> KfSponsorshipTypeUpdatedByNavigations { get; set; } = new List<KfSponsorshipType>();

    public virtual HrStaffMaster Staff { get; set; } = null!;

    public virtual ICollection<StudentRegistration> StudentRegistrationCreatedByNavigations { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<StudentRegistration> StudentRegistrationUpdatedByNavigations { get; set; } = new List<StudentRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationCreatedByNavigations { get; set; } = new List<UnUniversityRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationUpdatedByNavigations { get; set; } = new List<UnUniversityRegistration>();

    public virtual ICollection<UsersLoginRole> UsersLoginRoles { get; set; } = new List<UsersLoginRole>();

    public virtual ICollection<UsersLoginsLog> UsersLoginsLogs { get; set; } = new List<UsersLoginsLog>();
}
