namespace ScholarshipManagementAPI.DTOs.Common.Settings
{
    public class DashboardCardDto
    {
        public string Key { get; set; } = string.Empty;                         // "total_students"
        public string Title { get; set; } = string.Empty;                       // "Total Students"
        public int Value { get; set; }

        public decimal? GrowthPercentage { get; set; }

        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
