using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UnMasterCourse
{
    public long CourseId { get; set; }

    public long UniversityId { get; set; }

    public string CourseName { get; set; } = null!;

    public string? CourseCode { get; set; }

    public long CourseTypeId { get; set; }

    public long Duration { get; set; }

    public string DurationUnit { get; set; } = null!;

    public int NoSemester { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Remarks { get; set; }

    public int ApprovalStatus { get; set; }

    public long? ApprovedBy { get; set; }

    public virtual UsersLogin? ApprovedByNavigation { get; set; }

    public virtual UnMasterCourseType CourseType { get; set; } = null!;

    public virtual ICollection<UnCourseReq> UnCourseReqs { get; set; } = new List<UnCourseReq>();

    public virtual UnUniversityList University { get; set; } = null!;
}
