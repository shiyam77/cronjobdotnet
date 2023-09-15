using cronjob.Models;
using Microsoft.EntityFrameworkCore;

namespace cronjob.Authentication
{
    public class Appdbcontext : DbContext
    {
        public Appdbcontext(DbContextOptions<Appdbcontext> options) : base(options){ }
        //public DbSet<CurrencydataModel> CurrencyExchangeRates { get; set; }

        public DbSet<currencyExchangeRate> ExchangeRates { get; set; }

        public DbSet<CurrencyApiResponse> CurrencyApiResponse { get; set; }

    }
}
