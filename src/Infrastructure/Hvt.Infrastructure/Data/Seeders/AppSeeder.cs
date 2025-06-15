using Hvt.Data.Models;
using Hvt.Infrastructure.Repositories.DbContext;

namespace Hvt.Infrastructure.Data.Seeders
{
    public class AppSeeder
    {
        public static async Task SeedAsync(HvtTradeDbContext context)
        {
            if (!context.AppSettings.Any())
            {
                context.AppSettings.AddRange(AppSettings);
                await context.SaveChangesAsync();
            }
        }

        private static readonly List<AppSetting> AppSettings =
        [
            new AppSetting()
            {
                SettingKey = "CurrentCapital",
                SettingValue = "30000000",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        ];
    }
}
