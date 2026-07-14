using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterCourse;
using ScholarshipManagementAPI.Services.Interface.University;
using System;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CourseService : ICourseService
    {
        public Task<long> CreateAsync(MasterCourseRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(MasterCourseRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<MasterCourseRequestDto?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<MasterCourseRequestDto>> GetByFilterAsync(MasterCourseFilterDto filter, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }
    }
}
