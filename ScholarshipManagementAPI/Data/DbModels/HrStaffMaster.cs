using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class HrStaffMaster
{
    public long StaffId { get; set; }

    /// <summary>
    /// university, school, ngo
    /// </summary>
    public long StaffType { get; set; }

    public string StaffSalutation { get; set; } = null!;

    public string StaffFirstName { get; set; } = null!;

    public string StaffLastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? PermAddress { get; set; }

    public string? PermCity { get; set; }

    public string? PermZipCode { get; set; }

    public string? PermState { get; set; }

    public string? PremCountry { get; set; }

    public string OfficeEmail { get; set; } = null!;

    public string? PersonelEmail { get; set; }

    public string? Photo { get; set; }

    public string? MobileNo { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public long? UniversityId { get; set; }

    public long? SchoolId { get; set; }

    public long? NgoId { get; set; }

    public virtual KfSchool? School { get; set; }

    public virtual UsersModule StaffTypeNavigation { get; set; } = null!;

    public virtual ICollection<UnUniversityList> UnUniversityLists { get; set; } = new List<UnUniversityList>();

    public virtual UnUniversityList? University { get; set; }

    public virtual ICollection<UsersLogin> UsersLogins { get; set; } = new List<UsersLogin>();
}
