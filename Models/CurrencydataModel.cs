using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace cronjob.Models
{
    
    [Keyless]
    public class CurrencyApiResponse
    {
        public Dictionary<string, decimal> Data { get; set; }
    }

    public class currencyExchangeRate
    {
        [Key]
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
