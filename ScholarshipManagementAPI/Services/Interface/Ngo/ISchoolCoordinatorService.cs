using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.Administration.SchoolCoordinators;

namespace ScholarshipManagementAPI.Services.Interface.Ngo
{
    public interface ISchoolCoordinatorService
    {

        Task<long> CreateAsync(SchoolCoordinatorRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> UpdateAsync(SchoolCoordinatorRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> DeleteAsync(long staffId, LoggedInUserDto currentUser);

        Task<SchoolCoordinatorRequestDto> GetByIdAsync(long staffId);

        Task<PagedResultDto<SchoolCoordinatorRequestDto>> GetByFilterAsync(SchoolCoordinatorFilterDto filter);

    }
}
