using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IStudentRequirementService
    {
        //Task<long> CreateAsync(StudentRequirementRequestDto dto);
        //Task<bool> UpdateAsync(StudentRequirementRequestDto dto);


        Task<long> CreateStudentRequirementMapAsync(StudentRequirementMappingDto dto);
        Task<bool> UpdateStudentRequirementMapAsync(StudentRequirementMappingDto dto);

        Task<bool> UpdateStudentRequirementMapByUniversityAsync(StudentRequirementRequestDto dto, LoggedInUserDto currentUser);
        Task<bool> UpdateStudentRequirementMapByNgoAsync(StudentRequirementRequestDto dto, LoggedInUserDto currentUser);

        Task<bool> DeleteAsync(long id);

        Task<StudentRequirementRequestDto?> GetByIdAsync(long id);
        Task<PagedResultDto<StudentRequirementRequestDto>> GetByFilterAsync(StudentRequirementFilterDto filter);




        Task<string> UploadAsync(long studentReqId, long masterDocId, IFormFile file);
        Task<string> UploadAsync(UploadDocumentRequestDto dto);



        Task<List<StudentDocumentDto>> GetDocumentStatusAsync(long studentReqId);
        Task<List<StudentDocumentDto>> GetDocumentStatusAsync(DocumentStatusRequestDto request);

    }
}
