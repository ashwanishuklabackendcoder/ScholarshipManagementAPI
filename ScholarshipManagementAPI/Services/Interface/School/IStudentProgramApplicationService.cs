using Microsoft.AspNetCore.Http;
using ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IStudentProgramApplicationService
    {
        Task<List<CandidateProgramResponseDto>> GetCandidateProgramsAsync(long studentId);
        Task<long> ApplyAsync(long studentId, ApplyRequestDto dto, long userId);
        Task<bool> CancelApplicationAsync(long applicationId, long userId);
        Task<bool> SubmitApplicationAsync(long applicationId, long userId);
        Task<StudentProgramApplicationResponseDto?> GetApplicationAsync(long applicationId);
        Task<StudentProgramDocumentResponseDto> UploadDocumentAsync(long applicationId, long programDocumentId, long documentTypeId, IFormFile file, long userId);
        Task<bool> DeleteDocumentAsync(long applicationId, long documentId, long userId);
        Task<List<StudentProgramDocumentResponseDto>> GetDocumentsAsync(long applicationId);
        Task<List<StudentHistoryResponseDto>> GetHistoryAsync(long studentId);
    }
}
