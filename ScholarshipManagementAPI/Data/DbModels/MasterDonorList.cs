using System;
using System.Collections.Generic;

namespace ScholarshipManagementAPI.Data.DbModels;

public partial class MasterDonorList
{
    public long DonorId { get; set; }

    public string DonorName { get; set; } = null!;

    public string DonorCode { get; set; } = null!;

    public string? DonorEmail { get; set; }

    public string DonorPhone { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual ICollection<StudentReqList> StudentReqLists { get; set; } = new List<StudentReqList>();
}
