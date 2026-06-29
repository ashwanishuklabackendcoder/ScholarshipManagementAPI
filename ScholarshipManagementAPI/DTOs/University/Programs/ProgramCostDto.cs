namespace ScholarshipManagementAPI.DTOs.University.Programs
{
    public class ProgramCostDto
    {
        public long? ProgramCostId { get; set; }

        public long SponsorshipTypeId { get; set; }

        public string? SponsorshipTypeName { get; set; }


        // 1- One-time costs (FrequencyTypeId == 1)
        // 2- Recurring(Semester) costs (FrequencyTypeId == 2)
        public long? FrequencyTypeId { get; set; }

        public decimal Amount { get; set; }
    }
}
