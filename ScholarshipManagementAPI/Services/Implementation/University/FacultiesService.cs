using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Faculties;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class FacultiesService : IFacultiesService
    {
        private readonly AppDbContext _context;

        public FacultiesService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(FacultyRequestDto dto)
        {
            if (await _context.KfFaculties.AnyAsync(
                      x => x.FacultyName.ToLower() == dto.FacultyName.ToLower()
                      && x.UniversityId == dto.UniversityId))
            {
                throw new CustomException("Faculty with same name already exists");
            }

            var entity = new KfFaculty
            {
                UniversityId = dto.UniversityId,
                FacultyName = dto.FacultyName,
                FacultyCode = dto.FacultyCode,
                IsActive = true,

                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,       // always server-side
                CreatedBy = dto.CreatedBy ?? 0,                         // always server-side

                UpdatedBy = null,
                UpdatedDate = null
            };

            _context.KfFaculties.Add(entity);
            await _context.SaveChangesAsync();

            return entity.FacultyId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(FacultyRequestDto dto)
        {
            if (dto.FacultyId == null || dto.FacultyId == 0)
                return false;

            if (await _context.KfFaculties.AnyAsync(x =>
                      x.FacultyName.ToLower() == dto.FacultyName.ToLower()
                      && x.FacultyId != dto.FacultyId
                      && x.UniversityId == dto.UniversityId))
            {
                throw new CustomException("Faculty with same name already exists");
            }

            var entity = await _context.KfFaculties
                .FirstOrDefaultAsync(x => x.FacultyId == dto.FacultyId);

            if (entity == null)
                return false;


            entity.UniversityId = dto.UniversityId;
            entity.FacultyName = dto.FacultyName;
            entity.FacultyCode = dto.FacultyCode;

            entity.UpdatedDate = dto.UpdatedDate;     // always server-side
            entity.UpdatedBy = dto.UpdatedBy;         // always server-side

            // not updated
            // entity.IsActive = dto.IsActive;
            // entity.CreatedBy = dto.CreatedBy;
            // entity.CreatedDate = dto.CreatedDate;

            await _context.SaveChangesAsync();
            return true;
        }



        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.KfFaculties
                .FirstOrDefaultAsync(x => x.FacultyId == id);

            if (entity == null)
                return false;

            // Permanent delete
            //_context.KfFaculties.Remove(entity);

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<FacultyRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfFaculties
                .AsNoTracking()
                .Where(x => x.FacultyId == id)
                .Select(x => new FacultyRequestDto
                {
                    UniversityId = x.UniversityId,
                    UniversityName = x.University.UniversityName,

                    FacultyId = x.FacultyId,
                    FacultyName = x.FacultyName,
                    FacultyCode = x.FacultyCode,

                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation.LoginName,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedByNavigation == null ? null : x.UpdatedByNavigation.LoginName
                })
                .FirstOrDefaultAsync();
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<FacultyRequestDto>> GetByFilterAsync(FacultyFilterDto filter)
        {
            var query = _context.KfFaculties
                .AsNoTracking()
                .AsQueryable();


            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            if (filter.UniversityId.HasValue)
            {
                query = query.Where(x => x.UniversityId == filter.UniversityId.Value);
            }

            // filter date range filter

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.FacultyName.ToLower().Contains(search) ||
                    x.FacultyCode.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.FacultyId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new FacultyRequestDto
                {
                    UniversityId = x.UniversityId,
                    UniversityName = x.University.UniversityName,

                    FacultyId = x.FacultyId,
                    FacultyName = x.FacultyName,
                    FacultyCode = x.FacultyCode,

                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation.LoginName,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedByNavigation == null ? null : x.UpdatedByNavigation.LoginName
                })
                .ToListAsync();

            return new PagedResultDto<FacultyRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




        // ---------------- GET FACULTY PROGRAMS DASHBOARD ----------------

        public async Task<FacultyProgramsDashboardDto>GetFacultyProgramsDashboardAsync(long universityId)
        {
            var programsQuery = _context.KfPrograms
                .AsNoTracking()
                .Where(x =>
                    x.UniversityId == universityId &&
                    x.IsActive);

            var result = new FacultyProgramsDashboardDto
            {
                TotalFaculties = await _context.KfFaculties
                    .CountAsync(x =>
                        x.UniversityId == universityId &&
                        x.IsActive),

                AccreditedPrograms = await programsQuery
                    .CountAsync(x =>
                        x.AccreditationStatus ==
                        (byte)AccreditationStatusEnum.Accredited),

                UnderReviewPrograms = await programsQuery
                    .CountAsync(x =>
                        x.AccreditationStatus ==
                        (byte)AccreditationStatusEnum.Pending)
            };

            result.Faculties = await programsQuery
                .GroupBy(x => new
                {
                    x.FacultyId,
                    x.Faculty.FacultyName,
                    x.Faculty.FacultyCode
                })
                .Select(g => new FacultyProgramsSummaryDto
                {
                    FacultyId = g.Key.FacultyId,
                    FacultyName = g.Key.FacultyName,
                    FacultyCode = g.Key.FacultyCode,

                    TotalPrograms = g.Count(),

                    AverageSemesters =
                        Math.Round(
                            g.Average(x => (decimal)x.NumberOfSemesters),
                            1),

                    Programs = g
                        .OrderBy(x => x.ProgramName)
                        .Select(x => new FacultyProgramItemDto
                        {
                            ProgramId = x.ProgramId,
                            ProgramName = x.ProgramName,
                            ProgramCode = x.ProgramCode,

                            IsDraft = x.IsDraft,

                            AccreditationStatus =
                                x.AccreditationStatus,

                            StatusName =
                                x.IsDraft
                                    ? "Draft"
                                    : x.AccreditationStatus ==
                                      (byte)AccreditationStatusEnum.Pending
                                        ? "Under Review"
                                    : x.AccreditationStatus ==
                                      (byte)AccreditationStatusEnum.Accredited
                                        ? "Accredited"
                                    : "Rejected"
                        })
                        .ToList()
                })
                .OrderBy(x => x.FacultyName)
                .ToListAsync();

            return result;
        }

    }
}
