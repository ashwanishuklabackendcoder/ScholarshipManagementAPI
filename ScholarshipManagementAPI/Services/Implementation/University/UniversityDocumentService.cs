using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterDocuments;
using ScholarshipManagementAPI.Services.Interface.University;
using System;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class UniversityDocumentService : IUniversityDocumentService
    {
        public Task<long> CreateAsync(UniversityDocumentRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UniversityDocumentRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<UniversityDocumentRequestDto?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<UniversityDocumentRequestDto>> GetByFilterAsync(UniversityDocumentFilterDto filter, LoggedInUserDto currentUser)
        {
            throw new NotImplementedException();
        }
    }
}
