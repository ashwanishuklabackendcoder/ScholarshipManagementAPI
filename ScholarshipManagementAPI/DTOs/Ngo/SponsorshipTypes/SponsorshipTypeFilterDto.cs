using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes
{
    public class SponsorshipTypeFilterDto : BaseFilterDto
    {
        public long? SponsorshipTypeId { get; set; }

        public byte? FrequencyType { get; set; }

        public bool? IsActive { get; set; }
    }
}
