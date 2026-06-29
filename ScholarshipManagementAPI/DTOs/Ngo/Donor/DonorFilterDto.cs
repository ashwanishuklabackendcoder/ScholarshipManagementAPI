using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.Ngo.Donor
{
    public class DonorFilterDto : BaseFilterDto
    {
        public long? DonorId { get; set; }
        public string? DonorName { get; set; }
        public string? DonorCode { get; set; }
        public string? DonorEmail { get; set; }
        public string? DonorPhone { get; set; }
    }
}
