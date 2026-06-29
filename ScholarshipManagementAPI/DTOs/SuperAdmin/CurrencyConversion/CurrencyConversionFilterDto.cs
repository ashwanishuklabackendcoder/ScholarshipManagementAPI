using ScholarshipManagementAPI.DTOs.Common.Filter;

namespace ScholarshipManagementAPI.DTOs.SuperAdmin.CurrencyConversion
{
    public class CurrencyConversionFilterDto : BaseFilterDto
    {

        public long? CurrencyId { get; set; }

    }
}
