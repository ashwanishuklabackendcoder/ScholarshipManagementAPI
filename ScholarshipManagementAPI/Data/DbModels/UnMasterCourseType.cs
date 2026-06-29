using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UnMasterCourseType
{
    public long CourseTypeId { get; set; }

    public string CourseTypeName { get; set; } = null!;

    public long UniversityId { get; set; }

    public int ApprovalStatus { get; set; }

    public long? ApprovedBy { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual UsersLogin? ApprovedByNavigation { get; set; }

    public virtual ICollection<UnMasterCourse> UnMasterCourses { get; set; } = new List<UnMasterCourse>();

    public virtual UnUniversityList University { get; set; } = null!;
}
