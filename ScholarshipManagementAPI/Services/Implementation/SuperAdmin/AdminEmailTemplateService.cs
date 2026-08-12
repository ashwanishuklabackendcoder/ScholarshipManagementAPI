using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.AdminEmailTemplate;
using ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class AdminEmailTemplateService : IAdminEmailTemplateService
    {
        private readonly AppDbContext _context;
        public AdminEmailTemplateService(AppDbContext context)
        {
            _context = context;
        }

        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(AdminEmailTemplateRequestDto dto)
        {
            if (await _context.ZzAdminEmailTemplates
                .AnyAsync(x => x.TemplateName.ToLower() == dto.TemplateName.ToLower()))
            {
                throw new CustomException("Template name already exists");
            }


            var entity = new ZzAdminEmailTemplate
            {
                TemplateName = dto.TemplateName,
                Subject = dto.Subject,
                Template = dto.Template,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = dto.CreatedBy
            };

            _context.ZzAdminEmailTemplates.Add(entity);
            await _context.SaveChangesAsync();

            return entity.EmailTempId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(AdminEmailTemplateRequestDto dto)
        {
            if (dto.EmailTempId == null || dto.EmailTempId == 0)
                return false;

            if (await _context.ZzAdminEmailTemplates.AnyAsync(x =>
                      x.TemplateName.ToLower() == dto.TemplateName.ToLower()
                      && x.EmailTempId != dto.EmailTempId))
            {
                throw new CustomException("Template name already exists");
            }

            var entity = await _context.ZzAdminEmailTemplates
                .FirstOrDefaultAsync(x => x.EmailTempId == dto.EmailTempId);

            if (entity == null)
                return false;

            entity.TemplateName = dto.TemplateName;
            entity.Subject = dto.Subject;
            entity.Template = dto.Template;
            entity.IsActive = true;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedBy = dto.UpdatedBy;

            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzAdminEmailTemplates
                .FirstOrDefaultAsync(x => x.EmailTempId == id);

            if (entity == null)
                return false;

            // soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<AdminEmailTemplateRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzAdminEmailTemplates
                .AsNoTracking()
                .Where(x => x.EmailTempId == id && x.IsActive)
                .Select(x => new AdminEmailTemplateRequestDto
                {
                    EmailTempId = x.EmailTempId,
                    TemplateName = x.TemplateName,
                    Subject = x.Subject,
                    Template = x.Template,
                    IsActive = x.IsActive,

                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,

                    CreatedByName = x.CreatedByNavigation != null
                    ? x.CreatedByNavigation.Staff.StaffSalutation + " " +
                    x.CreatedByNavigation.Staff.StaffFirstName + " " +
                    x.CreatedByNavigation.Staff.StaffLastName
                    : null,

                    UpdatedByName = x.UpdatedByNavigation != null
                    ? x.UpdatedByNavigation.Staff.StaffSalutation + " " +
                    x.UpdatedByNavigation.Staff.StaffFirstName + " " +
                    x.UpdatedByNavigation.Staff.StaffLastName
                    : null
                })
                .FirstOrDefaultAsync();

        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<AdminEmailTemplateRequestDto>> GetByFilterAsync(AdminEmailTemplateFilterDto filter)
        {
            var query = _context.ZzAdminEmailTemplates
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.TemplateName.ToLower().Contains(search) ||
                    x.Subject.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.EmailTempId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new AdminEmailTemplateRequestDto
                {
                    EmailTempId = x.EmailTempId,
                    TemplateName = x.TemplateName,
                    Subject = x.Subject,
                    Template = x.Template,
                    IsActive = x.IsActive,

                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,

                    CreatedByName = x.CreatedByNavigation != null
                    ? x.CreatedByNavigation.Staff.StaffSalutation + " " +
                    x.CreatedByNavigation.Staff.StaffFirstName + " " +
                    x.CreatedByNavigation.Staff.StaffLastName
                    : null,

                    UpdatedByName = x.UpdatedByNavigation != null
                    ? x.UpdatedByNavigation.Staff.StaffSalutation + " " +
                    x.UpdatedByNavigation.Staff.StaffFirstName + " " +
                    x.UpdatedByNavigation.Staff.StaffLastName
                    : null
                })
                .ToListAsync();

            return new PagedResultDto<AdminEmailTemplateRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




    }
}
