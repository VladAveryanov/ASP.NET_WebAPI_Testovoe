using Microsoft.EntityFrameworkCore;

namespace testovoe.Models
{
    public class CurrencyContext: DbContext
    {
        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options) { }
        public DbSet<Currency> AllCurrencies { get; set; }
    }
}
