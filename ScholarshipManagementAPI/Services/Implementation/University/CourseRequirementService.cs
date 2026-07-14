using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.DTOs.University.CourseRequirement;
using ScholarshipManagementAPI.Services.Interface.University;
using System;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CourseRequirementService : ICourseRequirementService
    {
        public Task<long> CreateAsync(CourseRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CourseRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<CourseRequirementRequestDto?> GetByIdAsync(long id, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<CourseRequirementRequestDto>> GetByFilterAsync(CourseRequirementFilterDto filter, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<CourseRequirementEnrollmentDto>> GetEnrollmentsAsync(CourseRequirementFilterDto filter, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<EnrolledStudentDto>> GetEnrolledStudentsAsync(long? reqId, StudentRequirementFilterDto filter, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }
    }
}
