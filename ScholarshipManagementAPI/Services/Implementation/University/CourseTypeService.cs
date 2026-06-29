using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.MasterCourseType;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class CourseTypeService : ICourseTypeService
    {
        private readonly AppDbContext _context;

        public CourseTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(CourseTypeRequestDto dto)
        {
            var exists = await _context.UnMasterCourseTypes
                .AnyAsync(x => x.UniversityId == dto.UniversityId && x.IsActive &&
                          x.CourseTypeName.ToLower() == dto.CourseTypeName.ToLower());

            if (exists)
            {
                throw new CustomException(
                    "Course type with the same name already exists for this university");
            }

            var entity = new UnMasterCourseType
            {
                CourseTypeName = dto.CourseTypeName,
                UniversityId = dto.UniversityId,
                ApprovalStatus = (int)ApprovalStatus.Pending,  // pending by default
                ApprovedBy = null,                             // no approver at creation
                IsActive = true,                               // default to active
                CreatedDate = dto.CreatedDate,                 // always server-side
                CreatedBy = dto.CreatedBy                      // always server-side
            };

            _context.UnMasterCourseTypes.Add(entity);
            await _context.SaveChangesAsync();

            return entity.CourseTypeId;
        }


        public async Task<bool> UpdateAsync(CourseTypeRequestDto dto)
        {
            var entity = await _context.UnMasterCourseTypes
                .FirstOrDefaultAsync(x => x.CourseTypeId == dto.CourseTypeId);

            if (entity == null)
            {
                throw new CustomException("Course type not found");
            }

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Approved)
                throw new CustomException("Approved course type cannot be edited");

            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Rejected)
                throw new CustomException("Rejected course type cannot be edited");

            // 🔹 Uniqueness check (same university, same name, different record)
            var exists = await _context.UnMasterCourseTypes
                .AnyAsync(x =>
                    x.CourseTypeId != dto.CourseTypeId &&
                    x.UniversityId == dto.UniversityId && x.IsActive &&
                    x.CourseTypeName.ToLower() == dto.CourseTypeName.ToLower());

            if (exists)
            {
                throw new CustomException(
                    "Course type with the same name already exists for this university");
            }

            // 🔹 Update allowed fields
            entity.CourseTypeName = dto.CourseTypeName;
            // entity.UniversityId = dto.UniversityId;

            // entity.IsActive = dto.IsActive;

            // entity.ApprovalStatus = dto.ApprovalStatus;    // usually updated via ngo
            // entity.ApprovedBy = dto.ApprovedBy;            // usually updated via ngo
            // entity.CreatedDate = dto.CreatedDate;          // usually not updated
            // entity.CreatedBy = dto.CreatedBy;              // usually not updated

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UnMasterCourseTypes
                .FirstOrDefaultAsync(x => x.CourseTypeId == id && x.IsActive);

            if (entity == null)
                return false;

            if (entity.ApprovalStatus == (int)ApprovalStatus.Approved)
                throw new CustomException("Approved course types cannot be deleted");


            // ⭐ BUSINESS RULE CHECK (place here)
            if (entity.ApprovalStatus == (int)ApprovalStatus.Rejected)
                throw new CustomException("Rejected course type cannot be deleted");

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<CourseTypeRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UnMasterCourseTypes
                .AsNoTracking()
                .Where(x => x.CourseTypeId == id)
                .Select(x => new CourseTypeRequestDto
                {
                    CourseTypeId = x.CourseTypeId,
                    CourseTypeName = x.CourseTypeName,
                    UniversityId = x.UniversityId,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UniversityName = x.University.UniversityName,

                    ApprovalStatus = x.ApprovalStatus,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedByNavigation != null ? x.ApprovedByNavigation.LoginName : null
                })
                .FirstOrDefaultAsync();
        }


        public async Task<PagedResultDto<CourseTypeRequestDto>> GetByFilterAsync(CourseTypeFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.UnMasterCourseTypes
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
                    // School should NOT manage CourseType
                    // safest → return empty
                    query = query.Where(x => false);
                }
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
                    x.CourseTypeName.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.CourseTypeId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new CourseTypeRequestDto
                {
                    CourseTypeId = x.CourseTypeId,
                    CourseTypeName = x.CourseTypeName,
                    UniversityId = x.UniversityId,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UniversityName = x.University.UniversityName,

                    ApprovalStatus = x.ApprovalStatus,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedByNavigation != null ? x.ApprovedByNavigation.LoginName : null
                })
                .ToListAsync();

            return new PagedResultDto<CourseTypeRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
