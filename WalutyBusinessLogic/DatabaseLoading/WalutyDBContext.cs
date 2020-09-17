using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public class WalutyDBContext : IdentityDbContext<User>
    {
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CurrencyInfo> CurrencyInfos { get; set; }
        public virtual DbSet<UserCurrency> UsersCurrencies { get; set; }

        public WalutyDBContext(DbContextOptions<WalutyDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserCurrency>()
                .HasKey(uc => new { uc.CurrencyId, uc.UserId });
        }
    }
}
