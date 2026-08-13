using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.LanguageTranslation
{
    public class LanguageTranslationFilterDto : BaseFilterDto
    {
        public long? LabelId { get; set; }
        public long? LanguageId { get; set; }


        // Management filters
        public long? ModuleId { get; set; }
    }
}
