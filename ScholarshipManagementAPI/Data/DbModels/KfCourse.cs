using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfCourse
{
    public long CourseId { get; set; }

    public long UniversityId { get; set; }

    public string CourseCode { get; set; } = null!;

    public string CourseNameEn { get; set; } = null!;

    public string? CourseNameAr { get; set; }

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDraft { get; set; }

    public virtual KfUsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfCourseFaculty> KfCourseFaculties { get; set; } = new List<KfCourseFaculty>();

    public virtual ICollection<KfProgramCourse> KfProgramCourses { get; set; } = new List<KfProgramCourse>();

    public virtual UnUniversityRegistration University { get; set; } = null!;

    public virtual KfUsersLogin? UpdatedByNavigation { get; set; }
}
