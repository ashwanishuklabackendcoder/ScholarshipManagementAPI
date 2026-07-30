namespace ScholarshipManagementAPI.DTOs.Common.MarketingAdministrativeFee
{
    public class MarketingAdministrativeFeeHistoryDto
    {
        public long MarketingAdministrativeFeeId { get; set; }

        public decimal FeePercentage { get; set; }

        public bool IsCurrent { get; set; }

        public long CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }

        public string? UpdatedByName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

    }
}

