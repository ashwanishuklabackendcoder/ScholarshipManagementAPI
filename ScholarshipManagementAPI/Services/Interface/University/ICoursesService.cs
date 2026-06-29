using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Courses;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface ICoursesService
    {
        Task<long> CreateAsync(CourseRequestDto dto);

        Task<bool> UpdateAsync(CourseRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<CourseRequestDto?> GetByIdAsync(long id);

        Task<PagedResultDto<CourseRequestDto>>GetByFilterAsync(CourseFilterDto filter);
    }
}
