using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes;
using ScholarshipManagementAPI.DTOs.Ngo.StudentCategory;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Services.Implementation.Ngo
{
    public class StudentCategoryService : IStudentCategoryService
    {
        private readonly AppDbContext _context;

        public StudentCategoryService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<long> CreateAsync(StudentCategoryRequestDto dto)
        {
            var categoryName = dto.CategoryName.Trim();

            if (await _context.KfStudentCategories
                .AnyAsync(x => x.CategoryName.ToLower() == categoryName.ToLower()))
            {
                throw new CustomException("Student category with same name already exists.");
            }

            var entity = new KfStudentCategory
            {
                CategoryName = dto.CategoryName,
                DisplayOrder = dto.DisplayOrder,                  
                IsActive = true,

                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,       // always server-side
                CreatedBy = dto.CreatedBy ?? 0,                         // always server-side

                UpdatedBy = null,
                UpdatedDate = null
            };

            _context.KfStudentCategories.Add(entity);
            await _context.SaveChangesAsync();

            return entity.StudentCategoryId;
        }


        public async Task<bool> UpdateAsync(StudentCategoryRequestDto dto)
        {
            var entity = await _context.KfStudentCategories.FindAsync(dto.StudentCategoryId);
            if (entity == null)
            {
                throw new CustomException("Student category not found");
            }

            var categoryName = dto.CategoryName.Trim();

            if (await _context.KfStudentCategories.AnyAsync(x =>
                x.StudentCategoryId != dto.StudentCategoryId &&
                x.CategoryName.ToLower() == categoryName.ToLower()))
            {
                throw new CustomException("Student category with same name already exists.");
            }

            entity.CategoryName = dto.CategoryName;
            entity.DisplayOrder = dto.DisplayOrder;

            entity.UpdatedDate = dto.UpdatedDate;     // always server-side
            entity.UpdatedBy = dto.UpdatedBy;         // always server-side

            // not updated
            // entity.IsActive = dto.IsActive;
            // entity.CreatedBy = dto.CreatedBy;
            // entity.CreatedDate = dto.CreatedDate;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.KfStudentCategories
                .FirstOrDefaultAsync(x => x.StudentCategoryId == id);

            if (entity == null)
                return false;

            // Permanent delete
            //_context.KfStudentCategories.Remove(entity);

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<StudentCategoryRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfStudentCategories
                .AsNoTracking()
                .Where(x => x.StudentCategoryId == id)
                .Select(x => new StudentCategoryRequestDto
                {
                    StudentCategoryId = x.StudentCategoryId,
                    CategoryName = x.CategoryName,
                    DisplayOrder = x.DisplayOrder,

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



        public async Task<PagedResultDto<StudentCategoryRequestDto>> GetByFilterAsync(StudentCategoryFilterDto filter)
        {
            var query = _context.KfStudentCategories
                .AsNoTracking()
                .AsQueryable();


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
                    x.CategoryName.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderBy(x => x.StudentCategoryId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new StudentCategoryRequestDto
                {
                    StudentCategoryId = x.StudentCategoryId,
                    CategoryName = x.CategoryName,
                    DisplayOrder = x.DisplayOrder,

                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation.LoginName,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedByNavigation == null ? null : x.UpdatedByNavigation.LoginName
                })
                .ToListAsync();

            return new PagedResultDto<StudentCategoryRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




    }
}
