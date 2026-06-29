using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterCourseType;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface ICourseTypeService
    {
        Task<long> CreateAsync(CourseTypeRequestDto dto);
        Task<bool> UpdateAsync(CourseTypeRequestDto dto);
        Task<bool> DeleteAsync(long id);

        Task<CourseTypeRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<CourseTypeRequestDto>> GetByFilterAsync(CourseTypeFilterDto filter, LoggedInUserDto currentUser);
    }
}
