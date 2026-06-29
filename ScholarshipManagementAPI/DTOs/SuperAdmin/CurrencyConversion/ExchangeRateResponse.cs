namespace ScholarshipManagementAPI.DTOs.SuperAdmin.CurrencyConversion
{
    public class ExchangeRateResponse
    {
        public string result { get; set; } = string.Empty;
        public string base_code { get; set; } = string.Empty;
        public Dictionary<string, decimal> conversion_rates { get; set; } = new Dictionary<string, decimal>();
    }
}
