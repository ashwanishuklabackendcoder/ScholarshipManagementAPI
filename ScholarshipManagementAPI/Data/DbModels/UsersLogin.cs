using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UsersLogin
{
    public long LoginId { get; set; }

    public long StaffId { get; set; }

    public string LoginName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RecoveryEmail { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    public string? TempPassword { get; set; }

    public DateTime? TempPassDateTime { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<UsersLogin> InverseCreatedByNavigation { get; set; } = new List<UsersLogin>();

    public virtual ICollection<UsersLogin> InverseUpdatedByNavigation { get; set; } = new List<UsersLogin>();

    public virtual ICollection<KfCourse> KfCourseCreatedByNavigations { get; set; } = new List<KfCourse>();

    public virtual ICollection<KfCourse> KfCourseUpdatedByNavigations { get; set; } = new List<KfCourse>();

    public virtual ICollection<KfDocumentType> KfDocumentTypeCreatedByNavigations { get; set; } = new List<KfDocumentType>();

    public virtual ICollection<KfDocumentType> KfDocumentTypeUpdatedByNavigations { get; set; } = new List<KfDocumentType>();

    public virtual ICollection<KfFaculty> KfFacultyCreatedByNavigations { get; set; } = new List<KfFaculty>();

    public virtual ICollection<KfFaculty> KfFacultyUpdatedByNavigations { get; set; } = new List<KfFaculty>();

    public virtual ICollection<KfMarketingAdministrativeFee> KfMarketingAdministrativeFeeCreatedByNavigations { get; set; } = new List<KfMarketingAdministrativeFee>();

    public virtual ICollection<KfMarketingAdministrativeFee> KfMarketingAdministrativeFeeUpdatedByNavigations { get; set; } = new List<KfMarketingAdministrativeFee>();

    public virtual ICollection<KfProgram> KfProgramCreatedByNavigations { get; set; } = new List<KfProgram>();

    public virtual ICollection<KfProgramRegistrationWindow> KfProgramRegistrationWindowCreatedByNavigations { get; set; } = new List<KfProgramRegistrationWindow>();

    public virtual ICollection<KfProgramRegistrationWindow> KfProgramRegistrationWindowUpdatedByNavigations { get; set; } = new List<KfProgramRegistrationWindow>();

    public virtual ICollection<KfProgram> KfProgramUpdatedByNavigations { get; set; } = new List<KfProgram>();

    public virtual ICollection<KfSchool> KfSchoolAccreditationByNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSchool> KfSchoolCreatedByNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSchool> KfSchoolUpdatedByNavigations { get; set; } = new List<KfSchool>();

    public virtual ICollection<KfSponsorshipCategoryMapping> KfSponsorshipCategoryMappingCreatedByNavigations { get; set; } = new List<KfSponsorshipCategoryMapping>();

    public virtual ICollection<KfSponsorshipCategoryMapping> KfSponsorshipCategoryMappingUpdatedByNavigations { get; set; } = new List<KfSponsorshipCategoryMapping>();

    public virtual ICollection<KfSponsorshipStudentCategory> KfSponsorshipStudentCategoryCreatedByNavigations { get; set; } = new List<KfSponsorshipStudentCategory>();

    public virtual ICollection<KfSponsorshipStudentCategory> KfSponsorshipStudentCategoryUpdatedByNavigations { get; set; } = new List<KfSponsorshipStudentCategory>();

    public virtual ICollection<KfSponsorshipType> KfSponsorshipTypeCreatedByNavigations { get; set; } = new List<KfSponsorshipType>();

    public virtual ICollection<KfSponsorshipType> KfSponsorshipTypeUpdatedByNavigations { get; set; } = new List<KfSponsorshipType>();

    public virtual ICollection<KfStaff> KfStaffCreatedByNavigations { get; set; } = new List<KfStaff>();

    public virtual ICollection<KfStaffSchoolCoordinatorMapping> KfStaffSchoolCoordinatorMappingCreatedByNavigations { get; set; } = new List<KfStaffSchoolCoordinatorMapping>();

    public virtual ICollection<KfStaffSchoolCoordinatorMapping> KfStaffSchoolCoordinatorMappingUpdatedByNavigations { get; set; } = new List<KfStaffSchoolCoordinatorMapping>();

    public virtual ICollection<KfStaffUniversityCoordinatorMapping> KfStaffUniversityCoordinatorMappingCreatedByNavigations { get; set; } = new List<KfStaffUniversityCoordinatorMapping>();

    public virtual ICollection<KfStaffUniversityCoordinatorMapping> KfStaffUniversityCoordinatorMappingUpdatedByNavigations { get; set; } = new List<KfStaffUniversityCoordinatorMapping>();

    public virtual ICollection<KfStaff> KfStaffUpdatedByNavigations { get; set; } = new List<KfStaff>();

    public virtual ICollection<KfStudentAcademicRegistration> KfStudentAcademicRegistrationCreatedByNavigations { get; set; } = new List<KfStudentAcademicRegistration>();

    public virtual ICollection<KfStudentAcademicRegistration> KfStudentAcademicRegistrationUpdatedByNavigations { get; set; } = new List<KfStudentAcademicRegistration>();

    public virtual ICollection<KfStudentHistory> KfStudentHistories { get; set; } = new List<KfStudentHistory>();

    public virtual ICollection<KfStudentProgramApplication> KfStudentProgramApplicationCreatedByNavigations { get; set; } = new List<KfStudentProgramApplication>();

    public virtual ICollection<KfStudentProgramApplication> KfStudentProgramApplicationUpdatedByNavigations { get; set; } = new List<KfStudentProgramApplication>();

    public virtual ICollection<KfStudentProgramDocument> KfStudentProgramDocumentUpdatedByNavigations { get; set; } = new List<KfStudentProgramDocument>();

    public virtual ICollection<KfStudentProgramDocument> KfStudentProgramDocumentUploadedByNavigations { get; set; } = new List<KfStudentProgramDocument>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationCreatedByNavigations { get; set; } = new List<KfStudentRegistration>();

    public virtual ICollection<KfStudentRegistration> KfStudentRegistrationUpdatedByNavigations { get; set; } = new List<KfStudentRegistration>();

    public virtual KfStaff Staff { get; set; } = null!;

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationAccreditationByNavigations { get; set; } = new List<UnUniversityRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationCreatedByNavigations { get; set; } = new List<UnUniversityRegistration>();

    public virtual ICollection<UnUniversityRegistration> UnUniversityRegistrationUpdatedByNavigations { get; set; } = new List<UnUniversityRegistration>();

    public virtual UsersLogin? UpdatedByNavigation { get; set; }

    public virtual ICollection<UsersLoginRole> UsersLoginRoleCreatedByNavigations { get; set; } = new List<UsersLoginRole>();

    public virtual ICollection<UsersLoginRole> UsersLoginRoleLogins { get; set; } = new List<UsersLoginRole>();

    public virtual ICollection<UsersLoginRole> UsersLoginRoleUpdatedByNavigations { get; set; } = new List<UsersLoginRole>();

    public virtual ICollection<UsersLoginsLog> UsersLoginsLogs { get; set; } = new List<UsersLoginsLog>();

    public virtual ICollection<UsersModule> UsersModuleCreatedByNavigations { get; set; } = new List<UsersModule>();

    public virtual ICollection<UsersModule> UsersModuleUpdatedByNavigations { get; set; } = new List<UsersModule>();

    public virtual ICollection<UsersRole> UsersRoleCreatedByNavigations { get; set; } = new List<UsersRole>();

    public virtual ICollection<UsersRole> UsersRoleUpdatedByNavigations { get; set; } = new List<UsersRole>();
}
