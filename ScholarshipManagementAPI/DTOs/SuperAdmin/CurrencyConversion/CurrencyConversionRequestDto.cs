namespace ScholarshipManagementAPI.DTOs.SuperAdmin.CurrencyConversion
{
    public class CurrencyConversionRequestDto
    {
        public long? CurrencyConversionId { get; set; }    // null/0 = Create, >0 = Update

        public long CurrencyId { get; set; }

        public decimal Rates { get; set; }

        public DateOnly? FromDate { get; set; }           // optional (default = today)

        public string? Remarks { get; set; }              // optional (admin notes)


        public string CreatedBy { get; set; } = string.Empty;  // admin username

        public DateTime CreatedDate { get; set; }         // dd-mm-yyyy hh:mm:ss




        // Currency Info
        public string CurrencyCode { get; set; } = "";
        public string CurrencyName { get; set; } = "";
        public string CurrencySymbol { get; set; } = "";

        // NEW: Base Currency Info
        //public string BaseCurrencyCode { get; set; } = "";
        //public string BaseCurrencyName { get; set; } = "";
        //public string BaseCurrencySymbol { get; set; } = "";
    }

}

