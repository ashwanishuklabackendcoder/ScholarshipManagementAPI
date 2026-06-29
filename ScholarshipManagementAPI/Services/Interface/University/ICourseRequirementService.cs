using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.DTOs.University.CourseRequirement;

namespace ScholarshipManagementAPI.Services.Interface.University
{
    public interface ICourseRequirementService
    {
        Task<long> CreateAsync(CourseRequirementRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> UpdateAsync(CourseRequirementRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> DeleteAsync(long id);

        Task<CourseRequirementRequestDto?> GetByIdAsync(long id, LoggedInUserDto currentUser);
        Task<PagedResultDto<CourseRequirementRequestDto>> GetByFilterAsync(CourseRequirementFilterDto filter, LoggedInUserDto currentUser);



        Task<PagedResultDto<CourseRequirementEnrollmentDto>> GetEnrollmentsAsync(CourseRequirementFilterDto filter, LoggedInUserDto currentUser);

        Task<PagedResultDto<EnrolledStudentDto>> GetEnrolledStudentsAsync(long? reqId, StudentRequirementFilterDto filter, LoggedInUserDto currentUser);

    }
}
