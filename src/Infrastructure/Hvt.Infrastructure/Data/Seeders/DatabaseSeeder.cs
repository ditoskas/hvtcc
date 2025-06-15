using Hvt.Infrastructure.Repositories.DbContext;

namespace Hvt.Infrastructure.Data.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(HvtTradeDbContext context)
        {
            await AppSeeder.SeedAsync(context);
            await SymbolSeeder.SeedAsync(context);
        }
    }
}
