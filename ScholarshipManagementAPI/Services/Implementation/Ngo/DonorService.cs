using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.Donor;
using ScholarshipManagementAPI.DTOs.School.Students;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Services.Implementation.Ngo
{
    public class DonorService : IDonorService
    {
        private readonly AppDbContext _context;

        public DonorService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(DonorRequestDto dto)
        {

            if (await _context.MasterDonorLists
                .AnyAsync(x => x.DonorCode.ToLower() == dto.DonorCode.ToLower()))
            {
                throw new CustomException("Donor with same code already exists");
            }

            var entity = new MasterDonorList
            {
                DonorName = dto.DonorName,
                DonorCode = dto.DonorCode,
                DonorEmail = dto.DonorEmail,
                DonorPhone = dto.DonorPhone,
                Remarks = dto.Remarks,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate
            };

            _context.MasterDonorLists.Add(entity);
            await _context.SaveChangesAsync();

            return entity.DonorId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(DonorRequestDto dto)
        {
            if (dto.DonorId == null || dto.DonorId == 0)
                return false;

            if (await _context.MasterDonorLists.AnyAsync(x =>
                      x.DonorCode.ToLower() == dto.DonorCode.ToLower()
                      && x.DonorId != dto.DonorId))
            {
                throw new CustomException("Donor with same code already exists");
            }

            var entity = await _context.MasterDonorLists
                .FirstOrDefaultAsync(x => x.DonorId == dto.DonorId);

            if (entity == null)
                return false;

            // entity.DonorId = dto.DonorId.Value;

            entity.DonorName = dto.DonorName;
            entity.DonorCode = dto.DonorCode;
            entity.DonorEmail = dto.DonorEmail;
            entity.DonorPhone = dto.DonorPhone;
            entity.Remarks = dto.Remarks;

            // entity.CreatedDate = dto.CreatedDate;         // usually not updated
            // entity.CreatedBy = dto.CreatedBy;             // usually not updated

            await _context.SaveChangesAsync();
            return true;
        }


        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.MasterDonorLists
                .FirstOrDefaultAsync(x => x.DonorId == id);

            if (entity == null)
                return false;

            _context.MasterDonorLists.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<DonorRequestDto?> GetByIdAsync(long id)
        {
            return await _context.MasterDonorLists
                .Include(x => x.StudentReqLists)
                .AsNoTracking()
                .Where(x => x.DonorId == id)
                .Select(x => new DonorRequestDto
                {
                    DonorId = x.DonorId,
                    DonorName = x.DonorName,
                    DonorCode = x.DonorCode,
                    DonorEmail = x.DonorEmail,
                    DonorPhone = x.DonorPhone,
                    Remarks = x.Remarks,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    AssociatedStudentCount = x.StudentReqLists.Count
                })
                .FirstOrDefaultAsync();
        }



        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<DonorRequestDto>> GetByFilterAsync(DonorFilterDto filter)
        {
            var query = _context.MasterDonorLists
                .Include(x => x.StudentReqLists)
                .AsNoTracking()
                .AsQueryable();

            // Country filter
            if (filter.DonorId.HasValue)
            {
                query = query.Where(x => x.DonorId == filter.DonorId.Value);
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    x.DonorName.ToLower().Contains(search) ||
                    x.DonorCode.ToLower().Contains(search) ||
                    (x.DonorPhone != null && x.DonorPhone.ToLower().Contains(search)) ||
                    (x.DonorEmail != null && x.DonorEmail.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.DonorId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new DonorRequestDto
                {
                    DonorId = x.DonorId,
                    DonorName = x.DonorName,
                    DonorCode = x.DonorCode,
                    DonorEmail = x.DonorEmail,
                    DonorPhone = x.DonorPhone,
                    Remarks = x.Remarks,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,

                    AssociatedStudentCount = x.StudentReqLists.Count
                })
                .ToListAsync();

            return new PagedResultDto<DonorRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




    }
}
