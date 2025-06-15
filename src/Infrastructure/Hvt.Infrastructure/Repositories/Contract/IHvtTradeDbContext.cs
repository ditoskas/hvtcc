using Hvt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Hvt.Infrastructure.Repositories.Contract
{
    public interface IHvtTradeDbContext
    {
        public DbSet<AppSetting> AppSettings { get; }
        public DbSet<Symbol> Symbols { get; }
        public DbSet<Trade> Trades { get; }
    }
}
