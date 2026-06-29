using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.DocumentsTypes;
using ScholarshipManagementAPI.DTOs.Ngo.SponsorshipTypes;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Services.Implementation.Ngo
{
    public class SponsorshipTypesService : ISponsorshipTypesService
    {
        private readonly AppDbContext _context;

        public SponsorshipTypesService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(SponsorshipTypeRequestDto dto)
        {
            if (await _context.KfSponsorshipTypes
                .AnyAsync(x => x.SponsorshipName.ToLower() == dto.SponsorshipName.ToLower()))
            {
                throw new CustomException("Sponsorship type with same name already exists");
            }

            var entity = new KfSponsorshipType
            {
                SponsorshipName = dto.SponsorshipName,
                FrequencyType = dto.FrequencyType,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true,

                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,       // always server-side
                CreatedBy = dto.CreatedBy ?? 0,                         // always server-side

                UpdatedBy = null,
                UpdatedDate = null
            };

            _context.KfSponsorshipTypes.Add(entity);
            await _context.SaveChangesAsync();

            return entity.SponsorshipTypeId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(SponsorshipTypeRequestDto dto)
        {
            if (dto.SponsorshipTypeId == null || dto.SponsorshipTypeId == 0)
                return false;

            if (await _context.KfSponsorshipTypes.AnyAsync(x =>
                      x.SponsorshipName.ToLower() == dto.SponsorshipName.ToLower()
                      && x.SponsorshipTypeId != dto.SponsorshipTypeId))
            {
                throw new CustomException("Sponsorship type with same name already exists");
            }

            var entity = await _context.KfSponsorshipTypes
                .FirstOrDefaultAsync(x => x.SponsorshipTypeId == dto.SponsorshipTypeId);

            if (entity == null)
                return false;

            entity.SponsorshipName = dto.SponsorshipName;
            entity.FrequencyType = dto.FrequencyType;
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



        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.KfSponsorshipTypes
                .FirstOrDefaultAsync(x => x.SponsorshipTypeId == id);

            if (entity == null)
                return false;

            // Permanent delete
            //_context.KfSponsorshipTypes.Remove(entity);

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<SponsorshipTypeRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfSponsorshipTypes
                .AsNoTracking()
                .Where(x => x.SponsorshipTypeId == id)
                .Select(x => new SponsorshipTypeRequestDto
                {
                    SponsorshipTypeId = x.SponsorshipTypeId,
                    SponsorshipName = x.SponsorshipName,
                    FrequencyType = x.FrequencyType,
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



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<SponsorshipTypeRequestDto>> GetByFilterAsync(SponsorshipTypeFilterDto filter)
        {
            var query = _context.KfSponsorshipTypes
                .AsNoTracking()
                .AsQueryable();


            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            if (filter.FrequencyType.HasValue)
            {
                query = query.Where(x => x.FrequencyType == filter.FrequencyType.Value);
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.SponsorshipName.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderBy(x => x.SponsorshipTypeId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new SponsorshipTypeRequestDto
                {
                    SponsorshipTypeId = x.SponsorshipTypeId,
                    SponsorshipName = x.SponsorshipName,
                    FrequencyType = x.FrequencyType,
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

            return new PagedResultDto<SponsorshipTypeRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



    }
}
