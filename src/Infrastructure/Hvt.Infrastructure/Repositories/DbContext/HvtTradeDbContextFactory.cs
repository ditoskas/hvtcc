using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hvt.Infrastructure.Repositories.DbContext
{
    public class HvtTradeDbContextFactory : IDesignTimeDbContextFactory<HvtTradeDbContext>
        {
            public HvtTradeDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<HvtTradeDbContext>();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));

                return new HvtTradeDbContext(optionsBuilder.Options);
            }
        }
    }
    