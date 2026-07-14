using Microsoft.AspNetCore.Http;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRequirements;
using ScholarshipManagementAPI.Services.Interface.School;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class StudentRequirementService : IStudentRequirementService
    {
        public Task<long> CreateStudentRequirementMapAsync(StudentRequirementMappingDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentRequirementMapAsync(StudentRequirementMappingDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentRequirementMapByUniversityAsync(StudentRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentRequirementMapByNgoAsync(StudentRequirementRequestDto dto, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<StudentRequirementRequestDto?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<StudentRequirementRequestDto>> GetByFilterAsync(StudentRequirementFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadAsync(long studentReqId, long masterDocId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadAsync(UploadDocumentRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentDocumentDto>> GetDocumentStatusAsync(long studentReqId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentDocumentDto>> GetDocumentStatusAsync(DocumentStatusRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
