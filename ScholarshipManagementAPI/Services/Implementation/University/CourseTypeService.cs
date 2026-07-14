using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterCourseType;
using ScholarshipManagementAPI.Services.Interface.University;
using System;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CourseTypeService : ICourseTypeService
    {
        public Task<long> CreateAsync(CourseTypeRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CourseTypeRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<CourseTypeRequestDto?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<CourseTypeRequestDto>> GetByFilterAsync(CourseTypeFilterDto filter, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }
    }
}
