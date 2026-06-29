namespace ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings
{
    public class GeneralConfigDto
    {
        public string FullNameFormat { get; set; } = string.Empty;
        public string DateFormat { get; set; } = string.Empty;
        public string TimeFormat { get; set; } = string.Empty;


        public string BaseCurrencyName { get; set; } = string.Empty;
        public string BaseCurrencySymbol { get; set; } = string.Empty;
        public string BaseCurrencyCode { get; set; } = string.Empty;
    }
}
