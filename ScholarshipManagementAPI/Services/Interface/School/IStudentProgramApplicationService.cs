using Microsoft.AspNetCore.Http;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentProgramApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Interface.School
{
    public interface IStudentProgramApplicationService
    {
        // School Coordinator - Student Program Application

        // Get eligible programs for student application
        Task<List<CandidateProgramResponseDto>> GetCandidateProgramsAsync(long studentId, LoggedInUserDto currentUser);

        // Create a new application
        Task<long> ApplyAsync(long studentId, ApplyRequestDto dto, long userId, LoggedInUserDto currentUser);

        // Cancel a draft application
        Task<bool> CancelApplicationAsync(long applicationId, long userId, LoggedInUserDto currentUser);

        // Submit application for university review
        Task<bool> SubmitApplicationAsync(long applicationId, long userId, LoggedInUserDto currentUser);

        // Get application details
        Task<StudentProgramApplicationResponseDto?> GetApplicationAsync(long applicationId, LoggedInUserDto currentUser);


        // Upload application document
        Task<StudentProgramDocumentResponseDto> UploadDocumentAsync(long applicationId,long programDocumentId, long documentTypeId, IFormFile file, long userId, LoggedInUserDto currentUser);

        // Delete application document
        Task<bool> DeleteDocumentAsync(long applicationId, long documentId, long userId, LoggedInUserDto currentUser);




        // ============================
        // Managed by System
        // Viewed by All Modules
        // ============================


        // Get uploaded documents
        Task<List<StudentProgramDocumentResponseDto>> GetDocumentsAsync(long applicationId, LoggedInUserDto currentUser);

        // Get application history
        Task<List<StudentHistoryResponseDto>> GetHistoryAsync(long studentId, LoggedInUserDto currentUser);



        // ============================
        // Application Review
        // University / Committee
        // ============================

        // Search student applications
        Task<PagedResultDto<StudentProgramApplicationDto>> SearchAsync(StudentProgramApplicationFilterDto filter,LoggedInUserDto currentUser);

        // Get application details for review
        Task<StudentProgramApplicationDto?> GetByIdAsync(long applicationId,LoggedInUserDto currentUser);

        // Update application status
        Task<bool> ChangeStatusAsync(long applicationId,ChangeStudentProgramStatusDto dto, LoggedInUserDto currentUser);


    } 

}
