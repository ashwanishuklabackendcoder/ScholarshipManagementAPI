using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRegistration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IStudentRegistrationService
    {
        Task<long> CreateAsync(StudentRegistrationRequestDto dto);
        Task<bool> UpdateAsync(StudentRegistrationRequestDto dto);
        Task<bool> DeleteAsync(long id);
        Task<StudentRegistrationResponseDto?> GetByIdAsync(long id);
        Task<PagedResultDto<StudentRegistrationResponseDto>> GetByFilterAsync(StudentRegistrationFilterDto filter);
    }
}
