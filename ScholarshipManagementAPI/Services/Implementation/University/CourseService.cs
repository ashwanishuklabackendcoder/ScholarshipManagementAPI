using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterCourse;
using ScholarshipManagementAPI.DTOs.University.MasterCourseType;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<long> CreateAsync(MasterCourseRequestDto dto)
        {
            var exists = await _context.UnMasterCourses
                .AnyAsync(x => x.UniversityId == dto.UniversityId && x.IsActive &&
                          x.CourseName.ToLower() == dto.CourseName.ToLower());

            if (exists)
            {
                throw new CustomException(
                    "Course with the same name already exists for this university");
            }

            var entity = new UnMasterCourse
            {
                UniversityId = dto.UniversityId,
                CourseName = dto.CourseName,
                CourseCode = dto.CourseCode,
                CourseTypeId = dto.CourseTypeId,
                Duration = dto.Duration,
                DurationUnit = dto.DurationUnit,
                NoSemester = dto.NoSemester,
                Remarks = dto.Remarks,
                ApprovalStatus = (int)ApprovalStatus.Pending,  // pending by default
                ApprovedBy = null,                             // no approver at creation
                IsActive = true,                               // default to active
                CreatedDate = dto.CreatedDate,                 // always server-side
                CreatedBy = dto.CreatedBy                      // always server-side
            };

            _context.UnMasterCourses.Add(entity);
            await _context.SaveChangesAsync();

            return entity.CourseTypeId;
        }


        public async Task<bool> UpdateAsync(MasterCourseRequestDto dto)
        {
            var entity = await _context.UnMasterCourses
                .FirstOrDefaultAsync(x => x.CourseId == dto.CourseId);

            if (entity == null)
            {
                throw new CustomException("Course not found");
            }

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Approved)
                throw new CustomException("Approved course cannot be edited");

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Rejected)
                throw new CustomException("Rejected course cannot be edited");


            // 🔹 Uniqueness check (same university, same name, different record)
            var exists = await _context.UnMasterCourses
                .AnyAsync(x =>
                    x.CourseId != dto.CourseId &&
                    x.UniversityId == dto.UniversityId && x.IsActive &&
                    x.CourseName.ToLower() == dto.CourseName.ToLower());

            if (exists)
            {
                throw new CustomException(
                    "Course with the same name already exists for this university");
            }

            // entity.CourseId = dto.CourseId; // usually not updated

            entity.CourseName = dto.CourseName;
            // entity.UniversityId = dto.UniversityId; // usually not updated
            entity.CourseCode = dto.CourseCode;
            entity.CourseTypeId = dto.CourseTypeId;
            entity.Duration = dto.Duration;
            entity.DurationUnit = dto.DurationUnit;
            entity.NoSemester = dto.NoSemester;
            entity.Remarks = dto.Remarks;

            // entity.IsActive = dto.IsActive; 

            // entity.ApprovalStatus = dto.ApprovalStatus;   // usually updated via ngo
            // entity.ApprovedBy = dto.ApprovedBy;           // usually updated via ngo
            // entity.CreatedDate = dto.CreatedDate;         // usually not updated
            // entity.CreatedBy = dto.CreatedBy;             // usually not updated

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UnMasterCourses
                .FirstOrDefaultAsync(x => x.CourseId == id && x.IsActive);

            if (entity == null)
                return false;

            if (entity.ApprovalStatus == (int)ApprovalStatus.Approved)
                throw new CustomException("Approved course cannot be deleted");

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Rejected)
                throw new CustomException("Rejected course cannot be deleted");


            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<MasterCourseRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UnMasterCourses
                .AsNoTracking()
                .Where(x => x.CourseId == id)
                .Select(x => new MasterCourseRequestDto
                {
                    CourseId = x.CourseId,
                    UniversityId = x.UniversityId,
                    CourseName = x.CourseName,
                    CourseCode = x.CourseCode,
                    CourseTypeId = x.CourseTypeId,
                    Duration = x.Duration,
                    DurationUnit = x.DurationUnit,
                    NoSemester = x.NoSemester,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    Remarks = x.Remarks,
                    CourseTypeName = x.CourseType.CourseTypeName,
                    UniversityName = x.University.UniversityName,

                    ApprovalStatus = x.ApprovalStatus,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedByNavigation != null ? x.ApprovedByNavigation.LoginName : null
                })
                .FirstOrDefaultAsync();
        }



        public async Task<PagedResultDto<MasterCourseRequestDto>> GetByFilterAsync(MasterCourseFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.UnMasterCourses
                .AsNoTracking()
                .AsQueryable();

            // ---------- DATA SCOPE FILTER ----------
            if (currentUser.StaffType != StaffType.SuperAdmin &&
                currentUser.StaffType != StaffType.Ngo)
            {
                if (currentUser.StaffType == StaffType.University)
                {
                    query = query.Where(x => x.UniversityId == currentUser.UniversityId);
                }
                else if (currentUser.StaffType == StaffType.School)
                {
                    // School should not manage courses
                    query = query.Where(x => false);
                }
            }

            // CourseTypeId filter
            if (filter.CourseTypeId.HasValue)
            {
                query = query.Where(x => x.CourseTypeId == filter.CourseTypeId.Value);
            }

            // university filter
            if (filter.UniversityId.HasValue)
            {
                query = query.Where(x => x.UniversityId == filter.UniversityId.Value);
            }

            // Approved filter
            if (filter.ApprovalStatus.HasValue)
            {
                query = query.Where(x => x.ApprovalStatus == filter.ApprovalStatus.Value);
            }

            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.University.UniversityName.ToLower().Contains(search) ||
                    x.CourseName.ToLower().Contains(search) ||
                    x.CourseType.CourseTypeName.ToLower().Contains(search) 
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
                .Select(x => new MasterCourseRequestDto
                {
                    CourseId = x.CourseId,
                    UniversityId = x.UniversityId,
                    CourseName = x.CourseName,
                    CourseCode = x.CourseCode,
                    CourseTypeId = x.CourseTypeId,
                    Duration = x.Duration,
                    DurationUnit = x.DurationUnit,
                    NoSemester = x.NoSemester,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    Remarks = x.Remarks,
                    CourseTypeName = x.CourseType.CourseTypeName,
                    UniversityName = x.University.UniversityName,

                    ApprovalStatus = x.ApprovalStatus,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedByNavigation != null ? x.ApprovedByNavigation.LoginName : null
                })
                .ToListAsync();

            return new PagedResultDto<MasterCourseRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




    }
}
