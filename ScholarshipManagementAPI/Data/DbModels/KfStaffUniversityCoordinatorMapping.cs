using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfStaffUniversityCoordinatorMapping
{
    public long StaffUniversityCoordinatorMappingId { get; set; }

    public long StaffId { get; set; }

    public long UniversityId { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfStaff Staff { get; set; } = null!;

    public virtual KfUniversity University { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
