using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterCourse;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface ICourseService
    {
        Task<long> CreateAsync(MasterCourseRequestDto dto);
        Task<bool> UpdateAsync(MasterCourseRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<MasterCourseRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<MasterCourseRequestDto>> GetByFilterAsync(MasterCourseFilterDto filter, LoggedInUserDto currentUser);
    }
}
