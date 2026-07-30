using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.DTOs.Common.MarketingAdministrativeFee;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class MarketingAdministrativeFeeService : IMarketingAdministrativeFeeService
    {
        private readonly AppDbContext _context;


        public MarketingAdministrativeFeeService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<MarketingAdministrativeFeeResponseDto> GetCurrentAsync()
        {
            var entity = await _context.KfMarketingAdministrativeFees
                .AsNoTracking()
                .Where(x => x.IsCurrent)
                .Select(x => new MarketingAdministrativeFeeResponseDto
                {
                    MarketingAdministrativeFeeId = x.MarketingAdministrativeFeeId,
                    FeePercentage = x.FeePercentage,
                    IsCurrent = x.IsCurrent,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new CustomException("Marketing administrative fee not configured.");
            }

            return entity;
        }


        public async Task<bool> UpdateAsync(MarketingAdministrativeFeeRequestDto dto, LoggedInUserDto currentUser)
        {
            if (dto.FeePercentage < 0 || dto.FeePercentage > 100)
            {
                throw new CustomException("Fee percentage must be between 0 and 100.");
            }

            var utcNow = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var currentFee = await _context.KfMarketingAdministrativeFees
                    .FirstOrDefaultAsync(x => x.IsCurrent);

                // First configuration
                if (currentFee == null)
                {
                    _context.KfMarketingAdministrativeFees.Add(new KfMarketingAdministrativeFee
                    {
                        FeePercentage = dto.FeePercentage,
                        EffectiveFrom = utcNow,
                        EffectiveTo = null,
                        IsCurrent = true,
                        CreatedBy = currentUser.LoginId,
                        CreatedDate = utcNow
                    });

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }

                // No change
                if (currentFee.FeePercentage == dto.FeePercentage)
                {
                    await transaction.CommitAsync();
                    return true;
                }

                // Expire current version
                currentFee.IsCurrent = false;
                currentFee.EffectiveTo = utcNow;
                currentFee.UpdatedBy = currentUser.LoginId;
                currentFee.UpdatedDate = utcNow;

                // Create new current version
                _context.KfMarketingAdministrativeFees.Add(new KfMarketingAdministrativeFee
                {
                    FeePercentage = dto.FeePercentage,
                    EffectiveFrom = utcNow,
                    EffectiveTo = null,
                    IsCurrent = true,
                    CreatedBy = currentUser.LoginId,
                    CreatedDate = utcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<MarketingAdministrativeFeeHistoryDto>> GetHistoryAsync()
        {
            var entities = await _context.KfMarketingAdministrativeFees
                .AsNoTracking()
                .Include(x => x.CreatedByNavigation)
                    .ThenInclude(x => x.Staff)
                .Include(x => x.UpdatedByNavigation)
                    .ThenInclude(x => x.Staff)
                .OrderByDescending(x => x.EffectiveFrom)
                .ToListAsync();

            return entities.Select(x => new MarketingAdministrativeFeeHistoryDto
            {
                MarketingAdministrativeFeeId = x.MarketingAdministrativeFeeId,
                FeePercentage = x.FeePercentage,
                EffectiveFrom = x.EffectiveFrom,
                EffectiveTo = x.EffectiveTo,
                IsCurrent = x.IsCurrent,

                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                CreatedByName = UserDisplayHelper.GetFullName(
                    x.CreatedByNavigation?.Staff?.StaffSalutation,
                    x.CreatedByNavigation?.Staff?.StaffFirstName,
                    x.CreatedByNavigation?.Staff?.StaffLastName),

                UpdatedDate = x.UpdatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedByName = x.UpdatedByNavigation == null
                    ? null
                    : UserDisplayHelper.GetFullName(
                        x.UpdatedByNavigation.Staff.StaffSalutation,
                        x.UpdatedByNavigation.Staff.StaffFirstName,
                        x.UpdatedByNavigation.Staff.StaffLastName),

            }).ToList();

        }


    }
}
