using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes;
using ScholarshipManagementAPI.DTOs.Ngo.StudentCategory;

namespace ScholarshipManagementAPI.DTOs.Ngo.SponsorshipMatrix
{
    public class SponsorshipMatrixDto
    {
        public List<SponsorshipTypeRequestDto> SponsorshipTypes { get; set; } = new();

        public List<StudentCategoryRequestDto> StudentCategories { get; set; } = new();

        public List<SponsorshipCategoryMappingDto> Mappings { get; set; } = new();

    }
}
