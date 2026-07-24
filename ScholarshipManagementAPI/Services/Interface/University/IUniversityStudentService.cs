using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Students;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface IUniversityStudentService
    {
        Task<PagedResultDto<UniversityStudentRequestDto>> GetByFilterAsync(UniversityStudentFilterDto filter, LoggedInUserDto currentUser);

        Task<UniversityStudentRequestDto?> GetByIdAsync(long studentId,long loginId, LoggedInUserDto currentUser);


    }
}
