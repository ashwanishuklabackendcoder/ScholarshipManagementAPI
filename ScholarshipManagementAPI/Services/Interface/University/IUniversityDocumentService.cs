using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterDocuments;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IUniversityDocumentService
    {
        Task<long> CreateAsync(UniversityDocumentRequestDto dto);
        Task<bool> UpdateAsync(UniversityDocumentRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<UniversityDocumentRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<UniversityDocumentRequestDto>> GetByFilterAsync(UniversityDocumentFilterDto filter, LoggedInUserDto currentUser);
    }
}
