using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class StudentReqList
{
    public long StudentReqId { get; set; }

    public long StudentId { get; set; }

    public long ReqId { get; set; }

    public int? DocumentStatus { get; set; }

    public string? ReasonRejection { get; set; }

    public string? MissedDocuments { get; set; }

    public DateTime? SemesterStartDate { get; set; }

    public string? LetterAccepCode { get; set; }

    public long? UniStatusBy { get; set; }

    public DateTime? UniStatusDate { get; set; }

    public int? DaAdmissionStatus { get; set; }

    public long? DaStatusBy { get; set; }

    public DateTime? DaStatusDate { get; set; }

    public long? DonorId { get; set; }

    public double? TotalCost { get; set; }

    public string? CreateEmailBy { get; set; }

    public string? ReasonInProgress { get; set; }

    public int? UniAwardingstatus { get; set; }

    public double? UniAwardingstatusCost { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual MasterDonorList? Donor { get; set; }

    public virtual UnCourseReq Req { get; set; } = null!;

    public virtual StudentDatum Student { get; set; } = null!;

    public virtual ICollection<StudentDocument> StudentDocuments { get; set; } = new List<StudentDocument>();
}
