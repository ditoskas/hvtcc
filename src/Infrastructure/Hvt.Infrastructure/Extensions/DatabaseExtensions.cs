using Hvt.Infrastructure.Data.Seeders;
using Hvt.Infrastructure.Repositories.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hvt.Infrastructure.Extensions
{
    public class DatabaseExtensions
    {
        /// <summary>  
        /// Initializes the database asynchronously by creating a scope, migrating, and seeding the database.  
        /// </summary>  
        /// <param name="serviceProvider">The application's service provider.</param>  
        /// <returns>A task representing the asynchronous operation.</returns>  
        public static async Task InitialiseDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HvtTradeDbContext>();

            // Apply migrations  
            await dbContext.Database.MigrateAsync();
    
            // Seed the database  
            await DatabaseSeeder.SeedAsync(dbContext);
        }
    }
}
