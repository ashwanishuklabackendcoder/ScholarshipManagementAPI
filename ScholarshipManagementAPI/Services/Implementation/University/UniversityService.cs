using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.MasterSchool;
using ScholarshipManagementAPI.DTOs.University.MasterUniversity;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class UniversityService : IUniversityService
    {
        private readonly AppDbContext _context;

        public UniversityService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(UniversityRequestDto dto)
        {
            if (await _context.UnUniversityLists
                .AnyAsync(x => x.UniversityName.ToLower() == dto.UniversityName.ToLower()))
            {
                throw new CustomException("University with same name already exists");
            }

            var entity = new UnUniversityList
            {
                UniversityName = dto.UniversityName,
                IsActive = dto.IsActive,
                CountryId = dto.CountryId,
                IsApproved = dto.IsApproved,
                ApprovedBy = dto.ApprovedBy,
                Remarks = dto.Remarks,
                DefaultCurrencyId = dto.DefaultCurrencyId,

                CreatedDate = DateTime.UtcNow,     // always server-side
            };

            _context.UnUniversityLists.Add(entity);
            await _context.SaveChangesAsync();

            return entity.UniversityId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(UniversityRequestDto dto)
        {
            if (dto.UniversityId == null || dto.UniversityId == 0)
                return false;

            if (await _context.UnUniversityLists.AnyAsync(x =>
                      x.UniversityName.ToLower() == dto.UniversityName.ToLower()
                      && x.UniversityId != dto.UniversityId))
            {
                throw new CustomException("University with same name already exists");
            }

            var entity = await _context.UnUniversityLists
                .FirstOrDefaultAsync(x => x.UniversityId == dto.UniversityId);

            if (entity == null)
                return false;

            //entity.UniversityId = dto.UniversityId.Value;

            entity.UniversityName = dto.UniversityName;
            entity.IsActive = dto.IsActive;
            entity.CountryId = dto.CountryId;
            entity.Remarks = dto.Remarks;
            entity.DefaultCurrencyId = dto.DefaultCurrencyId;


            // entity.IsApproved = dto.IsApproved;
            // entity.ApprovedBy = dto.ApprovedBy;
            // CreatedDate NOT updated on purpose

            await _context.SaveChangesAsync();
            return true;
        }



        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UnUniversityLists
                .FirstOrDefaultAsync(x => x.UniversityId == id);

            if (entity == null)
                return false;

            //_context.UnUniversityLists.Remove(entity);

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<UniversityRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UnUniversityLists
                .AsNoTracking()
                .Where(x => x.UniversityId == id)
                .Select(x => new UniversityRequestDto
                {
                    UniversityId = x.UniversityId,
                    UniversityName = x.UniversityName,
                    IsActive = x.IsActive,
                    CountryId = x.CountryId,
                    Remarks = x.Remarks,
                    IsApproved = x.IsApproved,
                    ApprovedBy = x.ApprovedBy,

                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ApprovedByName = x.ApprovedByNavigation != null
                        ? x.ApprovedByNavigation.StaffFirstName + " " + x.ApprovedByNavigation.StaffLastName
                        : null,

                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = x.DefaultCurrencyId,
                })
                .FirstOrDefaultAsync();
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<UniversityRequestDto>> GetByFilterAsync(UniversityFilterDto filter)
        {
            var query = _context.UnUniversityLists
                .AsNoTracking()
                .AsQueryable();

            // Country filter
            if (filter.CountryId.HasValue)
            {
                query = query.Where(x => x.CountryId == filter.CountryId.Value);
            }

            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            // filter date range filter

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.UniversityName.ToLower().Contains(search) ||
                    (x.Remarks != null && x.Remarks.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.UniversityId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UniversityRequestDto
                {
                    UniversityId = x.UniversityId,
                    UniversityName = x.UniversityName,
                    IsActive = x.IsActive,
                    CountryId = x.CountryId,
                    Remarks = x.Remarks,
                    IsApproved = x.IsApproved,
                    ApprovedBy = x.ApprovedBy,

                    CountryName = x.Country != null ? x.Country.CountryName : null,
                    ApprovedByName = x.ApprovedByNavigation != null
                        ? x.ApprovedByNavigation.StaffFirstName + " " + x.ApprovedByNavigation.StaffLastName
                        : null,

                    CreatedDate = x.CreatedDate,
                    DefaultCurrencyId = x.DefaultCurrencyId,
                })
                .ToListAsync();

            return new PagedResultDto<UniversityRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }





    }
}
