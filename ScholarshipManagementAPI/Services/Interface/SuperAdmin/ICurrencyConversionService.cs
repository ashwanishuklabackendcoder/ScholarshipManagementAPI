using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.CurrencyConversion;

namespace ScholarshipManagementAPI.Services.Interface.SuperAdmin
{
    public interface ICurrencyConversionService
    {
        Task<CurrencySyncResultDto> SyncRatesAsync(string triggeredBy);

        Task<Dictionary<string, decimal>> GetLatestRates(string baseCurrency);




        Task<long> AddManualRate(CurrencyConversionRequestDto dto);
        Task<CurrencyConversionRequestDto?> GetByIdAsync(long id);
        Task<List<CurrencyConversionRequestDto>> GetCurrentCurrencyRateAsync();
        Task<PagedResultDto<CurrencyConversionRequestDto>> GetByFilterAsync(CurrencyConversionFilterDto filter);


        Task<decimal> GetRateAsync(string currencyCode);

        decimal ConvertToBase(decimal amount, string fromCurrency, string baseCurrency, decimal rate);
        decimal ConvertFromBase(decimal amount, string toCurrency, string baseCurrency, decimal rate);


        // UI → shows & accepts DEFAULT currency
        // API → converts using central CurrencyService
        // DB → stores ONLY base currency
        // API → converts back for response

        // UI decides WHAT currency  
        // API decides HOW to convert  
        // DB stores ONLY ONE currency

    }
}
