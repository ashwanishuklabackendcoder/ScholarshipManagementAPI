using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.DocumentsTypes;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IDocumentTypesService
    {
        Task<long> CreateAsync(DocumentTypeRequestDto dto);

        Task<bool> UpdateAsync(DocumentTypeRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<DocumentTypeRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<DocumentTypeRequestDto>>GetByFilterAsync(DocumentTypeFilterDto filter);
    }
}
