using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.DTOs.Ngo.Accreditation;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;
using System;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.Ngo
{
    public class AccreditationService : IAccreditationService
    {
        private readonly AppDbContext _context;

        public AccreditationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AccreditSchoolAsync(SchoolAccreditationDto dto)
        {
            var entity = await _context.KfSchools
                .FirstOrDefaultAsync(x => x.SchoolId == dto.SchoolId);

            if (entity == null)
                return false;

            // Only schools currently pending can be accredited/rejected
            if (entity.AccreditationStatus != (byte)AccreditationStatusEnum.Pending)
                throw new CustomException("Only schools currently pending can be accredited/rejected.");

            if (dto.AccreditationStatus != AccreditationStatusEnum.Accredited &&
                dto.AccreditationStatus != AccreditationStatusEnum.Rejected)
            {
                throw new CustomException("Invalid accreditation status.");
            }

            entity.AccreditationStatus = (byte)dto.AccreditationStatus;
            entity.AccreditationBy = dto.UpdatedBy;
            entity.AccreditationDate = DateTime.UtcNow;
            entity.CommitteeComment = dto.CommitteeComment;

            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AccreditUniversityAsync(UniversityAccreditationDto dto)
        {
            var entity = await _context.KfUniversities
                .FirstOrDefaultAsync(x => x.UniversityId == dto.UniversityId);

            if (entity == null)
                return false;

            // Only universities currently pending can be accredited/rejected
            if (entity.AccreditationStatus != (byte)AccreditationStatusEnum.Pending)
                throw new CustomException(
                    "Only universities currently pending can be accredited/rejected.");

            // Only Accredited or Rejected are valid final statuses
            if (dto.AccreditationStatus != AccreditationStatusEnum.Accredited &&
                dto.AccreditationStatus != AccreditationStatusEnum.Rejected)
            {
                throw new CustomException("Invalid accreditation status.");
            }

            entity.AccreditationStatus = (byte)dto.AccreditationStatus;
            entity.AccreditationBy = dto.UpdatedBy;
            entity.AccreditationDate = DateTime.UtcNow;
            entity.CommitteeComment = dto.CommitteeComment;

            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AccreditProgramAsync(ProgramAccreditationDto dto)
        {
            var entity = await _context.KfPrograms
                .FirstOrDefaultAsync(x => x.ProgramId == dto.ProgramId);

            if (entity == null)
                return false;

            // Only programs currently pending can be accredited/rejected
            if (entity.AccreditationStatus != (byte)AccreditationStatusEnum.Pending)
                throw new CustomException(
                    "Only programs currently pending can be accredited/rejected.");

            // Only Accredited or Rejected are valid final statuses
            if (dto.AccreditationStatus != AccreditationStatusEnum.Accredited &&
                dto.AccreditationStatus != AccreditationStatusEnum.Rejected)
            {
                throw new CustomException("Invalid accreditation status.");
            }

            entity.AccreditationStatus = (byte)dto.AccreditationStatus;
            entity.AccreditationBy = dto.UpdatedBy;
            entity.AccreditationDate = DateTime.UtcNow;
            entity.CommitteeComment = dto.CommitteeComment;

            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
