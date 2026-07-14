using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfCourseFaculty
{
    public long CourseFacultyId { get; set; }

    public long CourseId { get; set; }

    public long FacultyId { get; set; }

    public bool IsDraft { get; set; }

    public bool IsActive { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual KfCourse Course { get; set; } = null!;

    public virtual KfFaculty Faculty { get; set; } = null!;
}
