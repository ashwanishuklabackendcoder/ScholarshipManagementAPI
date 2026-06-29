using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IUniversityRegistrationService
    {
        Task<long> RegisterAsync(UniversityRegistrationDto dto);
        Task<PagedResultDto<UniversityRegistrationDto>> GetRegistrationsByStatusAsync(int approvalStatus, int pageNumber, int pageSize);
        Task<bool> ApproveRegistrationAsync(long id, int status, long approvedByUserId);
    }
}
