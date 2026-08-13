using Amazon.Runtime.Internal.Util;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings;
using ScholarshipManagementAPI.DTOs.SuperAdmin.Languages;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;
using static ScholarshipManagementAPI.Helper.Utilities.Constant;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;

        public LanguageService(AppDbContext context)
        {
            _context = context;
        }



        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(LanguageRequestDto dto)
        {
            if (await _context.ZzLanguages
                .AnyAsync(x => x.LanguageCode.ToLower() == dto.LanguageCode.ToLower()))
            {
                throw new CustomException("Language code already exists");
            }

            var entity = new ZzLanguage
            {
                LanguageName = dto.LanguageName,
                LanguageCode = dto.LanguageCode,
                CultureCode = dto.CultureCode,
                IsRtl = dto.IsRTL,
                IsDefault = false,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.ZzLanguages.Add(entity);
            await _context.SaveChangesAsync();

            return entity.LanguageId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(LanguageRequestDto dto)
        {
            if (dto.LanguageId == null || dto.LanguageId == 0)
                return false;

            if (await _context.ZzLanguages.AnyAsync(x =>
                      x.LanguageCode.ToLower() == dto.LanguageCode.ToLower()
                      && x.LanguageId != dto.LanguageId))
            {
                throw new CustomException("Language code already exists");
            }

            var entity = await _context.ZzLanguages
                .FirstOrDefaultAsync(x => x.LanguageId == dto.LanguageId);

            if (entity == null)
                return false;

            entity.LanguageCode = dto.LanguageCode;
            entity.LanguageName = dto.LanguageName;
            entity.CultureCode = dto.CultureCode;
            entity.IsRtl = dto.IsRTL;
            entity.IsDefault = false;
            entity.IsActive = true;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzLanguages
                .FirstOrDefaultAsync(x => x.LanguageId == id);

            if (entity == null)
                return false;

            //_context.ZzLanguages.Remove(entity);
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<LanguageRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzLanguages
                .AsNoTracking()
                .Where(x => x.LanguageId == id && x.IsActive)
                .Select(x => new LanguageRequestDto
                {
                    LanguageId = x.LanguageId,
                    LanguageName = x.LanguageName,
                    LanguageCode = x.LanguageCode,
                    CultureCode = x.CultureCode,
                    IsRTL = x.IsRtl,
                    IsDefault = x.IsDefault,
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
        public async Task<PagedResultDto<LanguageRequestDto>> GetByFilterAsync(LanguageFilterDto filter)
        {
            var query = _context.ZzLanguages
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (filter.IsDefault.HasValue)
            {
                query = query.Where(x => x.IsDefault == filter.IsDefault.Value);
            }

            if (filter.IsRtl.HasValue)
            {
                query = query.Where(x => x.IsRtl == filter.IsRtl.Value);
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.LanguageName.ToLower().Contains(search) ||
                    x.LanguageCode.ToLower().Contains(search) ||
                    (x.CultureCode != null && x.CultureCode.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderBy(x => x.LanguageId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new LanguageRequestDto
                {
                    LanguageId = x.LanguageId,
                    LanguageName = x.LanguageName,
                    LanguageCode = x.LanguageCode,
                    CultureCode = x.CultureCode,
                    IsRTL = x.IsRtl,
                    IsDefault = x.IsDefault,
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

            return new PagedResultDto<LanguageRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
