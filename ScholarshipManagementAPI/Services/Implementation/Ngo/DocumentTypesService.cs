using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.Ngo.DocumentsTypes;
using ScholarshipManagementAPI.DTOs.University.Faculties;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Ngo;

namespace ScholarshipManagementAPI.Services.Implementation.Ngo
{
    public class DocumentTypesService : IDocumentTypesService
    {
        private readonly AppDbContext _context;

        public DocumentTypesService(AppDbContext context)
        {
            _context = context;
        }


        // ---------------- CREATE ----------------
        public async Task<long> CreateAsync(DocumentTypeRequestDto dto)
        {
            if (await _context.KfDocumentTypes
                .AnyAsync(x => x.DocumentName.ToLower() == dto.DocumentName.ToLower()))
            {
                throw new CustomException("Document type with same name already exists");
            }

            var entity = new KfDocumentType
            {
                DocumentName = dto.DocumentName,
                IsDefault = dto.IsDefault,
                DefaultRequired = dto.DefaultRequired,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true,

                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,       // always server-side
                CreatedBy = dto.CreatedBy ?? 0,                         // always server-side

                UpdatedBy = null,
                UpdatedDate = null
            };

            _context.KfDocumentTypes.Add(entity);
            await _context.SaveChangesAsync();

            return entity.DocumentTypeId;
        }



        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(DocumentTypeRequestDto dto)
        {
            if (dto.DocumentTypeId == null || dto.DocumentTypeId == 0)
                return false;

            if (await _context.KfDocumentTypes.AnyAsync(x =>
                      x.DocumentName.ToLower() == dto.DocumentName.ToLower()
                      && x.DocumentTypeId != dto.DocumentTypeId))
            {
                throw new CustomException("Document type with same name already exists");
            }

            var entity = await _context.KfDocumentTypes
                .FirstOrDefaultAsync(x => x.DocumentTypeId == dto.DocumentTypeId);

            if (entity == null)
                return false;

            entity.DocumentName = dto.DocumentName;
            entity.IsDefault = dto.IsDefault;
            entity.DefaultRequired = dto.DefaultRequired;
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
            var entity = await _context.KfDocumentTypes
                .FirstOrDefaultAsync(x => x.DocumentTypeId == id);

            if (entity == null)
                return false;

            // Permanent delete
            //_context.KfDocumentTypes.Remove(entity);

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        // ---------------- GET BY ID ----------------
        public async Task<DocumentTypeRequestDto?> GetByIdAsync(long id)
        {
            return await _context.KfDocumentTypes
                .AsNoTracking()
                .Where(x => x.DocumentTypeId == id)
                .Select(x => new DocumentTypeRequestDto
                {
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,

                    IsDefault = x.IsDefault,
                    DefaultRequired = x.DefaultRequired,
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
        public async Task<PagedResultDto<DocumentTypeRequestDto>> GetByFilterAsync(DocumentTypeFilterDto filter)
        {
            var query = _context.KfDocumentTypes
                .AsNoTracking()
                .AsQueryable();


            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            if (filter.IsDefault.HasValue)
            {
                query = query.Where(x => x.IsDefault == filter.IsDefault.Value);
            }

            if (filter.DefaultRequired.HasValue)
            {
                query = query.Where(x => x.DefaultRequired == filter.DefaultRequired.Value);
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.DocumentName.ToLower().Contains(search)
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.DocumentTypeId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new DocumentTypeRequestDto
                {
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,

                    IsDefault = x.IsDefault,
                    DefaultRequired = x.DefaultRequired,
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

            return new PagedResultDto<DocumentTypeRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


    }
}
