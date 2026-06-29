using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.Ngo.DocumentsTypes
{
    public class DocumentTypeFilterDto : BaseFilterDto
    {
        public long? DocumentTypeId { get; set; }

        public bool? IsDefault { get; set; }

        public bool? DefaultRequired { get; set; }

        public bool? IsActive { get; set; }
    }
}
