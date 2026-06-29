namespace ScholarshipManagementAPI.DTOs.SuperAdmin.Label
{
    public class LanguageLabelsDto
    {
        public string Language { get; set; } = string.Empty;

        public bool Rtl { get; set; } = false;

        public int Version { get; set; }

        public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();
   
    }
}
