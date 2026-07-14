using ScholarshipManagementAPI.DTOs.Ngo.Accreditation;
using ScholarshipManagementAPI.Services.Interface.Ngo;
using System;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.Ngo
{
    public class AccreditationService : IAccreditationService
    {
        public Task<bool> ApproveCourseTypeAsync(long id, int approvalStatus, long approvedBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApproveCourseAsync(long id, int approvalStatus, long approvedBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApproveCourseRequirementAsync(long id, int approvalStatus, long approvedBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApproveSchoolAsync(long schoolId, int approvalStatus, long approvedBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AccreditateProgram(ProgramAccreditationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
