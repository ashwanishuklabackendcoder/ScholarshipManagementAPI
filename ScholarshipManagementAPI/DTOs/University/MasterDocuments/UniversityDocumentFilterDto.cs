using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.University.MasterDocuments
{
    public class UniversityDocumentFilterDto : BaseFilterDto
    {
        public long? UniversityId { get; set; }

        public bool? IsActive { get; set; }

    }
}
