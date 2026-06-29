using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Label;
using ScholarshipManagementAPI.Helper.Enums;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface ILabelService
    {
        Task<long> CreateAsync(LabelRequestDto dto);
        Task<bool> UpdateAsync(LabelRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<LabelRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<LabelRequestDto>> GetByFilterAsync(LabelFilterDto filter);

        Task<LanguageLabelsDto> GetTranslations(LanguageCode language);

    }
}
