using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class MasterCurrrencyService : IMasterCurrencyService
    {
        private readonly AppDbContext _context;

        public MasterCurrrencyService(AppDbContext context)
        {
            _context = context;
        }

        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(MasterCurrencyRequestDto dto)
        {
            if (await _context.ZzMasterCurrencies
                .AnyAsync(x => x.CurrencyName.ToLower() == dto.CurrencyName.ToLower()))
            {
                throw new CustomException("Currency with same CurrencyName already exists");
            }


            var entity = new ZzMasterCurrency
            {
                CurrencyName = dto.CurrencyName,
                CurrencyCode = dto.CurrencyCode,
                CurrencySymbol = dto.CurrencySymbol,
                CurrencyFracUnit = dto.CurrencyFracUnit,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow     // always server-side
            };

            _context.ZzMasterCurrencies.Add(entity);
            await _context.SaveChangesAsync();

            return entity.CurrencyId;
        }

        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(MasterCurrencyRequestDto dto)
        {
            if (dto.CurrencyId == null || dto.CurrencyId == 0)
                return false;

            if (await _context.ZzMasterCurrencies.AnyAsync(x =>
                      x.CurrencyName.ToLower() == dto.CurrencyName.ToLower()
                      && x.CurrencyId != dto.CurrencyId))
            {
                throw new CustomException("Dropdown with same DisplayText already exists");
            }

            var entity = await _context.ZzMasterCurrencies
                .FirstOrDefaultAsync(x => x.CurrencyId == dto.CurrencyId);

            if (entity == null)
                return false;

            entity.CurrencyName = dto.CurrencyName;
            entity.CurrencyCode = dto.CurrencyCode;
            entity.CurrencySymbol = dto.CurrencySymbol;
            entity.CurrencyFracUnit = dto.CurrencyFracUnit;
            entity.IsActive = dto.IsActive;
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzMasterCurrencies
                .FirstOrDefaultAsync(x => x.CurrencyId == id);

            if (entity == null)
                return false;

            _context.ZzMasterCurrencies.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<MasterCurrencyRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzMasterCurrencies
                .AsNoTracking()
                .Where(x => x.CurrencyId == id)
                .Select(x => new MasterCurrencyRequestDto
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.CurrencyName,
                    CurrencyCode = x.CurrencyCode,
                    CurrencySymbol = x.CurrencySymbol,
                    CurrencyFracUnit = x.CurrencyFracUnit,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<MasterCurrencyRequestDto>> GetByFilterAsync(MasterCurrencyFilterDto filter)
        {
            var query = _context.ZzMasterCurrencies.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CurrencyName))
                query = query.Where(x => x.CurrencyName.Contains(filter.CurrencyName));

            if (!string.IsNullOrWhiteSpace(filter.CurrencyCode))
                query = query.Where(x => x.CurrencyCode.Contains(filter.CurrencyCode));

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.CurrencyName.ToLower().Contains(search) ||
                    x.CurrencyCode.ToLower().Contains(search) ||
                    x.CurrencySymbol.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.CurrencyId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new MasterCurrencyRequestDto
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.CurrencyName,
                    CurrencyCode = x.CurrencyCode,
                    CurrencySymbol = x.CurrencySymbol,
                    CurrencyFracUnit = x.CurrencyFracUnit,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<MasterCurrencyRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



    }
}
