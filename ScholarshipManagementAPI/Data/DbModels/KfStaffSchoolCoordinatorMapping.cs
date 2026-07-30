using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfStaffSchoolCoordinatorMapping
{
    public long StaffSchoolCoordinatorMappingId { get; set; }

    public long StaffId { get; set; }

    public long SchoolId { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfSchool School { get; set; } = null!;

    public virtual KfStaff Staff { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
