using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.Administration.UniversityCoordinators;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface IUniversityCoordinatorService
    {

        Task<long> CreateAsync(UniversityCoordinatorRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> UpdateAsync(UniversityCoordinatorRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> DeleteAsync(long staffId, LoggedInUserDto currentUser);

        Task<UniversityCoordinatorRequestDto> GetByIdAsync(long staffId);

        Task<PagedResultDto<UniversityCoordinatorRequestDto>> GetByFilterAsync(UniversityCoordinatorFilterDto filter);

    }
}

