using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class UnCourseReq
{
    public long ReqId { get; set; }

    public long CourseId { get; set; }

    public string AcademicYear { get; set; } = null!;

    public string RequiredDocuments { get; set; } = null!;

    public string HsDivision { get; set; } = null!;

    public int NoSeats { get; set; }

    public double? CollegeScore { get; set; }

    public bool? IsActive { get; set; }

    public string? TzStuCombi { get; set; }

    public double? RegistrationCost { get; set; }

    public double? TutionCost { get; set; }

    public double? TextBookCost { get; set; }

    public double? AccomoCost { get; set; }

    public double? TravellingCost { get; set; }

    public double? TransportCost { get; set; }

    public double? DocuAttestCost { get; set; }

    public double? VisaResiCost { get; set; }

    public DateTime? ReqStartDate { get; set; }

    public DateTime? ReqEndDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int ApprovalStatus { get; set; }

    public long? ApprovedBy { get; set; }

    public virtual UsersLogin? ApprovedByNavigation { get; set; }

    public virtual UnMasterCourse Course { get; set; } = null!;

    public virtual ICollection<StudentReqList> StudentReqLists { get; set; } = new List<StudentReqList>();
}
