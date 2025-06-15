using Hvt.Data.Models;
using Hvt.Infrastructure.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hvt.Infrastructure.Repositories.DbContext
{
    public class HvtTradeDbContext(DbContextOptions<HvtTradeDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options), IHvtTradeDbContext
    {
        public DbSet<AppSetting> AppSettings => Set<AppSetting>();
        public DbSet<Symbol> Symbols => Set<Symbol>();
        public DbSet<Trade> Trades => Set<Trade>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
