using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.MasterCountry;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class MasterCountryService : IMasterCountryService
    {
        private readonly AppDbContext _context;

        public MasterCountryService(AppDbContext context)
        {
            _context = context;
        }

        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(MasterCountryRequestDto dto)
        {
            if (await _context.ZzMasterCountries
                .AnyAsync(x => x.CountryName.ToLower() == dto.CountryName.ToLower()))
            {
                throw new CustomException("Country with same CountryName already exists");
            }


            var entity = new ZzMasterCountry
            {
                CountryName = dto.CountryName,
                CountryIsdCode = dto.CountryIsdCode,
                CountryAlphaCode3 = dto.CountryAlphaCode3,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.ZzMasterCountries.Add(entity);
            await _context.SaveChangesAsync();

            return entity.CountryId;
        }


        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(MasterCountryRequestDto dto)
        {
            if (dto.CountryId == null || dto.CountryId == 0)
                return false;

            if (await _context.ZzMasterCountries.AnyAsync(x =>
                      x.CountryName.ToLower() == dto.CountryName.ToLower()
                      && x.CountryId != dto.CountryId))
            {
                throw new CustomException("Dropdown with same DisplayText already exists");
            }

            var entity = await _context.ZzMasterCountries
                .FirstOrDefaultAsync(x => x.CountryId == dto.CountryId);

            if (entity == null)
                return false;

            entity.CountryName = dto.CountryName;
            entity.CountryIsdCode = dto.CountryIsdCode;
            entity.CountryAlphaCode3 = dto.CountryAlphaCode3;
            entity.IsActive = true;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.ZzMasterCountries
                .FirstOrDefaultAsync(x => x.CountryId == id);

            if (entity == null)
                return false;

            entity.IsActive = false;
            //_context.ZzMasterCountries.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }


        // ---------------- GET BY ID ----------------
        public async Task<MasterCountryRequestDto?> GetByIdAsync(long id)
        {
            return await _context.ZzMasterCountries
                .AsNoTracking()
                .Where(x => x.CountryId == id && x.IsActive)
                .Select(x => new MasterCountryRequestDto
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    CountryIsdCode = x.CountryIsdCode,
                    CountryAlphaCode3 = x.CountryAlphaCode3,
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
        public async Task<PagedResultDto<MasterCountryRequestDto>> GetByFilterAsync(MasterCountryFilterDto filter)
        {
            var query = _context.ZzMasterCountries
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CountryName))
                query = query.Where(x => x.CountryName.Contains(filter.CountryName));

            if (filter.CountryIsdCode.HasValue)
                query = query.Where(x => x.CountryIsdCode == filter.CountryIsdCode);

            if (!string.IsNullOrWhiteSpace(filter.CountryAlphaCode3))
                query = query.Where(x => x.CountryAlphaCode3 == filter.CountryAlphaCode3);


            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);

            int? isdCode = null;
            if (int.TryParse(filter.SearchText, out var parsedIsd))
            {
                isdCode = parsedIsd;
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.CountryName.ToLower().Contains(search)
                    || (x.CountryAlphaCode3 != null && x.CountryAlphaCode3.ToLower().Contains(search))
                    || (isdCode.HasValue && x.CountryIsdCode == isdCode.Value)
                );
            }



            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderBy(x => x.CountryName);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new MasterCountryRequestDto
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    CountryIsdCode = x.CountryIsdCode,
                    CountryAlphaCode3 = x.CountryAlphaCode3,
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

            return new PagedResultDto<MasterCountryRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
