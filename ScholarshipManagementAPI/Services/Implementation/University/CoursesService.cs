using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.Courses;
using ScholarshipManagementAPI.DTOs.University.Faculties;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CoursesService : ICoursesService
    {
        private readonly AppDbContext _context;

        public CoursesService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(CourseRequestDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (await _context.KfCourses
                    .AnyAsync(x => x.CourseNameEn.ToLower() == dto.CourseNameEn.ToLower() 
                    && x.UniversityId == dto.UniversityId))
                {
                    throw new CustomException("Course with same name already exists");
                }

                var entity = new KfCourse
                {
                    UniversityId = dto.UniversityId,
                    CourseCode = dto.CourseCode,
                    CourseNameEn = dto.CourseNameEn,
                    CourseNameAr = dto.CourseNameAr,
                    IsActive = true,

                    CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,
                    CreatedBy = dto.CreatedBy ?? 0,

                    UpdatedBy = null,
                    UpdatedDate = null
                };

                _context.KfCourses.Add(entity);

                await _context.SaveChangesAsync();

                if (dto.FacultyIds != null && dto.FacultyIds.Any())
                {
                    var courseFaculties = dto.FacultyIds
                        .Distinct()
                        .Select(facultyId => new KfCourseFaculty
                        {
                            CourseId = entity.CourseId,
                            FacultyId = facultyId
                        })
                        .ToList();

                    _context.KfCourseFaculties.AddRange(courseFaculties);

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return entity.CourseId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(CourseRequestDto dto)
        {
            if (dto.CourseId == null || dto.CourseId == 0)
                return false;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (await _context.KfCourses.AnyAsync(x =>
                    x.CourseNameEn.ToLower() == dto.CourseNameEn.ToLower()
                    && x.CourseId != dto.CourseId
                    && x.UniversityId == dto.UniversityId))
                {
                    throw new CustomException("Course with same name already exists");
                }

                var entity = await _context.KfCourses
                    .FirstOrDefaultAsync(x => x.CourseId == dto.CourseId);

                if (entity == null)
                    return false;

                entity.UniversityId = dto.UniversityId;
                entity.CourseCode = dto.CourseCode;
                entity.CourseNameEn = dto.CourseNameEn;
                entity.CourseNameAr = dto.CourseNameAr;

                entity.UpdatedDate = dto.UpdatedDate;
                entity.UpdatedBy = dto.UpdatedBy;

                await _context.SaveChangesAsync();

                // Remove old mappings
                var existingMappings = await _context.KfCourseFaculties
                    .Where(x => x.CourseId == entity.CourseId)
                    .ToListAsync();

                if (existingMappings.Any())
                {
                    _context.KfCourseFaculties.RemoveRange(existingMappings);
                    await _context.SaveChangesAsync();
                }

                // Add new mappings
                if (dto.FacultyIds != null && dto.FacultyIds.Any())
                {
                    var newMappings = dto.FacultyIds
                        .Distinct()
                        .Select(facultyId => new KfCourseFaculty
                        {
                            CourseId = entity.CourseId,
                            FacultyId = facultyId
                        })
                        .ToList();

                    _context.KfCourseFaculties.AddRange(newMappings);

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.KfCourses
                .FirstOrDefaultAsync(x => x.CourseId == id);

            if (entity == null)
                return false;

            // Permanent delete
            //_context.KfCourses.Remove(entity);

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<CourseRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfCourses
                .AsNoTracking()
                .Where(x => x.CourseId == id)
                .Select(x => new CourseRequestDto
                {
                    UniversityId = x.UniversityId,
                    UniversityName = x.University.UniversityName,

                    CourseId = x.CourseId,
                    CourseCode = x.CourseCode,
                    CourseNameEn = x.CourseNameEn,
                    CourseNameAr = x.CourseNameAr,

                    Faculties = x.KfCourseFaculties
                    .Select(cf => new CourseFacultyDto
                    {
                        FacultyId = cf.FacultyId,
                        FacultyName = cf.Faculty.FacultyName
                    })
                    .ToList(),

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
        public async Task<PagedResultDto<CourseRequestDto>> GetByFilterAsync(CourseFilterDto filter)
        {
            var query = _context.KfCourses
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

            if (filter.FacultyId.HasValue)
            {
                query = query.Where(x => x.KfCourseFaculties.Any(cf => cf.FacultyId == filter.FacultyId.Value));
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.CourseNameEn.ToLower().Contains(search) ||
                    (x.CourseNameAr != null && x.CourseNameAr.ToLower().Contains(search)) ||
                    x.CourseCode.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.CourseId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new CourseRequestDto
                {
                    UniversityId = x.UniversityId,
                    UniversityName = x.University.UniversityName,

                    CourseId = x.CourseId,
                    CourseCode = x.CourseCode,
                    CourseNameEn = x.CourseNameEn,
                    CourseNameAr = x.CourseNameAr,

                    Faculties = x.KfCourseFaculties
                    .Select(cf => new CourseFacultyDto
                    {
                        FacultyId = cf.FacultyId,
                        FacultyName = cf.Faculty.FacultyName
                    })
                    .ToList(),

                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation.LoginName,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedByNavigation == null ? null : x.UpdatedByNavigation.LoginName
                })
                .ToListAsync();

            return new PagedResultDto<CourseRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



    }
}
