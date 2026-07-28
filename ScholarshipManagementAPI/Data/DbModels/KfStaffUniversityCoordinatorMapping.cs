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

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual HrStaffMaster Staff { get; set; } = null!;

    public virtual UnUniversityRegistration University { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
