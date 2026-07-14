using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class KfFaculty
{
    public long FacultyId { get; set; }

    public long UniversityId { get; set; }

    public string FacultyName { get; set; } = null!;

    public string FacultyCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDraft { get; set; }

    public virtual UsersLogin CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KfCourseFaculty> KfCourseFaculties { get; set; } = new List<KfCourseFaculty>();

    public virtual ICollection<KfProgram> KfPrograms { get; set; } = new List<KfProgram>();

    public virtual UnUniversityRegistration University { get; set; } = null!;

    public virtual UsersLogin? UpdatedByNavigation { get; set; }
}
