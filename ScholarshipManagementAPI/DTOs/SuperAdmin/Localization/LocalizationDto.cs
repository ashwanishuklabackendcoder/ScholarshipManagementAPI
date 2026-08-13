namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Localization
{
    public class LocalizationDto
    {
        public long LanguageId { get; set; }

        public string LanguageCode { get; set; } = string.Empty;

        public string CultureCode { get; set; } = string.Empty;

        public bool IsRTL { get; set; }

        public Dictionary<string, string> Translations { get; set; } = new();
    }
}
