namespace ScholarshipManagementAPI.DTOs.Common.Settings
{
    public class DashboardDto
    {
        // Fixed (core business metrics)
        public int NominatedCandidates { get; set; }
        public int AcceptedApplications { get; set; }
        public int SponsoredCandidates { get; set; }
        public int RejectedApplications { get; set; }



        // Monthly counts (optional but useful)
        public int NominatedThisMonth { get; set; }
        public int AcceptedThisMonth { get; set; }
        public int SponsoredThisMonth { get; set; }
        public int RejectedThisMonth { get; set; }

        // Growth (status-wise)
        public decimal NominatedGrowthPercentage { get; set; }
        public decimal AcceptedGrowthPercentage { get; set; }
        public decimal SponsoredGrowthPercentage { get; set; }
        public decimal RejectedGrowthPercentage { get; set; }



        // Existing (keep if needed globally)
        public int NewStudentsThisMonth { get; set; }
        public int ApplicationsThisMonth { get; set; }

        public decimal StudentsGrowthPercentage { get; set; }
        public decimal ApplicationsGrowthPercentage { get; set; }


        // NEW: Dynamic cards (future-proof)
        public List<DashboardCardDto> Cards { get; set; } = new();


    }
}
