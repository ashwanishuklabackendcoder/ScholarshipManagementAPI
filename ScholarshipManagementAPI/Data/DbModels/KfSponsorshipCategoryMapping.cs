using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfSponsorshipCategoryMapping
{
    public long MappingId { get; set; }

    public long SponsorshipTypeId { get; set; }

    public long StudentCategoryId { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual KfSponsorshipType SponsorshipType { get; set; } = null!;

    public virtual KfStudentCategory StudentCategory { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
