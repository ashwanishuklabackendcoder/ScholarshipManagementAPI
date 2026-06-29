using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.HrStaff;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.CurrencyConversion;
using ScholarshipManagementAPI.DTOs.SuperAdmin.GeneralSettings;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;
using System.Net.Http;
using System.Runtime;

namespace ScholarshipManagementAPI.Services.Implementation.SuperAdmin
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly IGeneralSettingsService _generalSettingsService;
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;


        public CurrencyConversionService(
            IGeneralSettingsService generalSettingsService,
            AppDbContext context,
            HttpClient httpClient, 
            IConfiguration config)
        {
            _generalSettingsService = generalSettingsService;
            _context = context;
            _httpClient = httpClient;
            _config = config;
        }


        public async Task<Dictionary<string, decimal>> GetLatestRates(string baseCurrency)
        {
            var apiKey = _config["CurrencyApi:ApiKey"];

            var url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{baseCurrency}";

            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);

            if (response == null || response.conversion_rates == null)
                throw new Exception("Invalid API response");

            var result = new Dictionary<string, decimal>();

            foreach (var item in response.conversion_rates)
            {
                var code = item.Key;
                var apiRate = item.Value;

                if (apiRate == 0)
                    continue;

                //if (code == baseCurrency || apiRate == 0)
                //    continue;

                // IMPORTANT: Convert rate
                // var convertedRate = 1 / apiRate;
                var convertedRate = apiRate;

                result[code] = Math.Round(convertedRate, 4);
            }

            return result;
        }



        public async Task<CurrencySyncResultDto> SyncRatesAsync(string triggeredBy)
        {
            int inserted = 0;
            int skipped = 0;

            // Step 1: Base Currency
            var config = await _generalSettingsService.GetGeneralConfigAsync();
            var baseCurrency = config.BaseCurrencyCode;

            // Step 2: API Call
            var rates = await GetLatestRates(baseCurrency);

            // Step 3: Mapping
            var currencyMap = await GetCurrencyCodeMapAsync();

            // Step 4: fetch latest rates from DB for comparison
            var latestRates = await _context.AcCurrencyConversions
                .GroupBy(x => x.CurrencyId)
                .Select(g => new
                {
                    CurrencyId = g.Key,
                    Rate = g.OrderByDescending(x => x.FromDate)
                            .Select(x => x.Rates)
                            .FirstOrDefault()
                })
                .ToDictionaryAsync(x => x.CurrencyId, x => x.Rate);

            // Step 5: Insert or Skip
            var newEntries = new List<AcCurrencyConversion>();

            foreach (var rate in rates)
            {
                var code = rate.Key.ToUpper();

                if (!currencyMap.ContainsKey(code))
                {
                    skipped++;
                    continue;
                }

                var currencyId = currencyMap[code];
                var newRate = rate.Value;

                if (latestRates.TryGetValue(currencyId, out var lastRate))
                {
                    if (Math.Abs(lastRate - newRate) < 0.0001m)
                    {
                        skipped++;
                        continue;
                    }
                }

                newEntries.Add(new AcCurrencyConversion
                {
                    CurrencyId = currencyId,
                    Rates = newRate,
                    FromDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = triggeredBy,
                    Remarks = $"Synced from API on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}"
                });

                inserted++;
            }

            if (newEntries.Any())
            {
                _context.AcCurrencyConversions.AddRange(newEntries);
                await _context.SaveChangesAsync();
            }


            return new CurrencySyncResultDto
            {
                Inserted = inserted,
                Skipped = skipped,
                Total = rates.Count
            };
        }


        public async Task<long> AddManualRate(CurrencyConversionRequestDto dto)
        {
            var entity = new AcCurrencyConversion
            {
                CurrencyId = dto.CurrencyId,
                Rates = dto.Rates,
                FromDate = dto.FromDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                CreatedDate = dto.CreatedDate,
                CreatedBy = dto.CreatedBy,
                Remarks = dto.Remarks
            };

            _context.AcCurrencyConversions.Add(entity);
            await _context.SaveChangesAsync();

            return entity.CurrencyConversionId;
        }


        // ---------------- GET BY ID ----------------
        public async Task<CurrencyConversionRequestDto?> GetByIdAsync(long id)
        {
            return await _context.AcCurrencyConversions
                .Include(x => x.Currency) // Include related currency data
                .AsNoTracking()
                .Where(x => x.CurrencyConversionId == id)
                .Select(x => new CurrencyConversionRequestDto
                {
                    CurrencyConversionId = x.CurrencyConversionId,
                    CurrencyId = x.CurrencyId,
                    Rates = x.Rates,
                    FromDate = x.FromDate,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    Remarks = x.Remarks,
                    CurrencyCode = x.Currency.CurrencyCode,
                    CurrencyName = x.Currency.CurrencyName,
                    CurrencySymbol = x.Currency.CurrencySymbol
                })
                .FirstOrDefaultAsync();

        }


        // ---------------- GET CURRENT RATES (LATEST PER CURRENCY) ----------------
        public async Task<List<CurrencyConversionRequestDto>> GetCurrentCurrencyRateAsync()
        {
            // Step 1: Base Currency
            var config = await _generalSettingsService.GetGeneralConfigAsync();
            var baseCurrencyCode = config.BaseCurrencyCode;
            var baseCurrencyName = config.BaseCurrencyName;
            var baseCurrencySymbol = config.BaseCurrencySymbol;

            // STEP 1: Get latest records per currency (DB query)
            var latestEntities = await _context.AcCurrencyConversions
                .Include(x => x.Currency) // ✅ LOAD RELATED DATA
                .GroupBy(x => x.CurrencyId)
                .Select(g => g
                    .OrderByDescending(x => x.FromDate)
                    .ThenByDescending(x => x.CreatedDate)
                    .First())
                .AsNoTracking()
                .ToListAsync();

            // STEP 2: Map in memory (NO EF issues)
            var result = latestEntities.Select(x => new CurrencyConversionRequestDto
            {
                CurrencyConversionId = x.CurrencyConversionId,
                CurrencyId = x.CurrencyId,
                Rates = x.Rates,
                FromDate = x.FromDate,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                Remarks = x.Remarks,

                CurrencyCode = x.Currency?.CurrencyCode ?? "",
                CurrencyName = x.Currency?.CurrencyName ?? "",
                CurrencySymbol = x.Currency?.CurrencySymbol ?? "",

                //BaseCurrencyCode = baseCurrencyCode,
                //BaseCurrencyName = baseCurrencyName,
                //BaseCurrencySymbol = baseCurrencySymbol
            }).ToList();

            return result;
        }


        // ---------------- GET ALL FILTER ----------------
        public async Task<PagedResultDto<CurrencyConversionRequestDto>> GetByFilterAsync(CurrencyConversionFilterDto filter)
        {
            var query = _context.AcCurrencyConversions
                .Include(x => x.Currency) // Include related currency data
                .AsNoTracking()
                .AsQueryable();

            /* Filter by CurrencyId */
            if (filter.CurrencyId.HasValue)
            {
                var currencyId = filter.CurrencyId.Value;
                query = query.Where(x => x.CurrencyId == currencyId);
            }

            /* Global Search */
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.Currency.CurrencyName.ToLower().Contains(search) ||
                    x.Currency.CurrencyCode.ToLower().Contains(search) ||
                    x.Currency.CurrencySymbol.ToLower().Contains(search) ||
                    x.Rates.ToString().ToLower().Contains(search) ||
                    (x.Remarks != null && x.Remarks.ToLower().Contains(search))
                );
            }


            // ---------- Total Count (before pagination) ----------
            var totalCount = await query.CountAsync();

            // ---------- Ordering ----------
            query = query.OrderByDescending(x => x.CurrencyConversionId);

            // ---------- Pagination rule ----------
            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new CurrencyConversionRequestDto
                {
                    CurrencyConversionId = x.CurrencyConversionId,
                    CurrencyId = x.CurrencyId,
                    Rates = x.Rates,
                    FromDate = x.FromDate,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    Remarks = x.Remarks,
                    CurrencyCode = x.Currency.CurrencyCode,
                    CurrencyName = x.Currency.CurrencyName,
                    CurrencySymbol = x.Currency.CurrencySymbol
                })
                .ToListAsync();

            return new PagedResultDto<CurrencyConversionRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }




        // ---------------- CURRENCY CONVERSION LOGIC ----------------
        public decimal ConvertToBase(decimal amount, string fromCurrency, string baseCurrency, decimal rate)
        {
            var result = fromCurrency == baseCurrency ? amount : amount / rate;

            return Math.Round(result, 4); 
        }


        // ---------------- Convert from base currency to target currency ----------------
        public decimal ConvertFromBase(decimal amount, string toCurrency, string baseCurrency, decimal rate)
        {
            var result = toCurrency == baseCurrency ? amount : amount * rate;

            return Math.Round(result, 2);
        }


        public async Task<decimal> GetRateAsync(string currencyCode)
        {
            return await _context.AcCurrencyConversions
                .Where(x => x.Currency.CurrencyCode == currencyCode)
                .OrderByDescending(x => x.FromDate)
                .Select(x => x.Rates)
                .FirstOrDefaultAsync();
        }


        private async Task<Dictionary<string, long>> GetCurrencyCodeMapAsync()
        {
            return await _context.ZzMasterCurrencies
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToDictionaryAsync(x => x.CurrencyCode, x => x.CurrencyId);
        }


    }
}
