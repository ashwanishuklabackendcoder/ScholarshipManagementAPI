using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.University.CourseRequirement;
using ScholarshipManagementAPI.DTOs.University.MasterDocuments;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Enums;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.University;

namespace ScholarshipManagementAPI.Services.Implementation.University
{
    public class UniversityDocumentService : IUniversityDocumentService
    {
        private readonly AppDbContext _context;
        public UniversityDocumentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(UniversityDocumentRequestDto dto)
        {
            if (await _context.UnMasterDocs
                .AnyAsync(x => x.DocumentName == dto.DocumentName && x.UniversityId == dto.UniversityId))
            {
                throw new CustomException("Document with same name already exists");
            }

            var entity = new UnMasterDoc
            {
                UniversityId = dto.UniversityId,
                IsDownloadable = dto.IsDownloadable,
                DocumentName = dto.DocumentName,
                FileName = dto.FileName,
                DocType = dto.DocType,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            };

            _context.UnMasterDocs.Add(entity);
            await _context.SaveChangesAsync();

            return entity.UniversityDocsId;
        }



        public async Task<bool> UpdateAsync(UniversityDocumentRequestDto dto)
        {
            var entity = await _context.UnMasterDocs
                .FirstOrDefaultAsync(x => x.UniversityDocsId == dto.UniversityDocsId);

            if (entity == null)
            {
                throw new CustomException("University document not found");
            }

            if (await _context.UnMasterDocs
                .AnyAsync(x => x.DocumentName == dto.DocumentName 
                && x.UniversityId == dto.UniversityId 
                && x.UniversityDocsId != dto.UniversityDocsId))
            {
                throw new CustomException("Document with same name already exists");
            }

            // UniversityDocsId usually should NOT be changed
            // entity.UniversityDocsId = dto.UniversityDocsId;

            entity.UniversityId = dto.UniversityId;
            entity.IsDownloadable = dto.IsDownloadable;
            entity.DocumentName = dto.DocumentName;
            entity.FileName = dto.FileName;
            entity.DocType = dto.DocType;

            // entity.IsActive = dto.IsActive;
            // entity.CreatedDate = dto.CreatedDate; // usually not updated

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.UnMasterDocs
                .FirstOrDefaultAsync(x => x.UniversityDocsId == id && x.IsActive == true);

            if (entity == null)
                return false;

            // Soft delete
            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<UniversityDocumentRequestDto?> GetByIdAsync(long id)
        {
            return await _context.UnMasterDocs
                .AsNoTracking()
                .Where(x => x.UniversityDocsId == id)
                .Include(x => x.University)
                .Select(x => new UniversityDocumentRequestDto
                {
                    UniversityDocsId = x.UniversityDocsId,
                    UniversityId = x.UniversityId,
                    IsDownloadable = x.IsDownloadable,
                    DocumentName = x.DocumentName,
                    FileName = x.FileName,
                    DocType = x.DocType,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,

                    UniversityName = x.University.UniversityName
                })
                .FirstOrDefaultAsync();
        }



        public async Task<PagedResultDto<UniversityDocumentRequestDto>> GetByFilterAsync(UniversityDocumentFilterDto filter, LoggedInUserDto currentUser)
        {
            var query = _context.UnMasterDocs
                .AsNoTracking()
                .Include(x => x.University)
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
                    // School should only view documents
                    query = query.Where(x => true);
                }
            }


            // Active status filter
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            // university status filter
            if (filter.UniversityId.HasValue)
            {
                query = query.Where(x => x.UniversityId == filter.UniversityId.Value);
            }


            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.University.UniversityName.ToLower().Contains(search) ||
                    (x.FileName != null && x.FileName.ToLower().Contains(search)) ||
                    x.DocumentName.ToLower().Contains(search) 
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.UniversityDocsId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new UniversityDocumentRequestDto
                {
                    UniversityDocsId = x.UniversityDocsId,
                    UniversityId = x.UniversityId,
                    IsDownloadable = x.IsDownloadable,
                    DocumentName = x.DocumentName,
                    FileName = x.FileName,
                    DocType = x.DocType,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,

                    UniversityName = x.University.UniversityName
                })
                .ToListAsync();

            return new PagedResultDto<UniversityDocumentRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




    }
}
