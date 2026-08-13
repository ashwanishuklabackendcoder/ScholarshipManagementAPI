namespace ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation
{
    public class LanguageTranslationItemDto
    {
        public long LanguageId { get; set; }

        public string LanguageName { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string? Value { get; set; }

        public bool IsTranslated { get; set; }
    }
}
