using Hvt.Infrastructure.Repositories.DbContext;
using Symbol = Hvt.Data.Models.Symbol;

namespace Hvt.Infrastructure.Data.Seeders
{
    public class SymbolSeeder
    {
        public static async Task SeedAsync(HvtTradeDbContext context)
        {
            if (!context.Symbols.Any())
            {
                context.Symbols.AddRange(Symbols);
                await context.SaveChangesAsync();
            }
        }

        private static readonly List<Symbol> Symbols =
        [
            new Symbol()
            {
                Name = "BTCUSDT",
                BinanceSymbol = "BTCUSDT",
                PolygonTicker = "BTC",
                EquityPercentageToBet = 0.1m,
                ProfitTargetPercentage = 0.3m,
                StopLossTargetPercentage = 0.1m,
                LeverageToTrade = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        ];
    }
}
