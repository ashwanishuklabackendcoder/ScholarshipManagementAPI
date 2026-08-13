namespace ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation
{
    public class LanguageTranslationManagementDto
    {
        public long LabelId { get; set; }
        public long? ModuleId { get; set; }
        public string? ModuleName { get; set; }

        // Label key (unique identifier for the label)
        public string LabelKey { get; set; } = string.Empty;

        // English / Master Value
        public string EnglishMasterValue { get; set; } = string.Empty;

        public List<LanguageTranslationItemDto> Translations { get; set; } = new();
    }
}
