using Binance.Client.Extensions;
using Hvt.Infrastructure.Handlers;
using Hvt.Infrastructure.HostedServices;
using Hvt.Infrastructure.Interceptors;
using Hvt.Infrastructure.Repositories.Contract;
using Hvt.Infrastructure.Repositories;
using Hvt.Infrastructure.Repositories.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Polygon.Client.Extensions;

namespace Hvt.Infrastructure.Extensions
{
    public static class HvtInfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["Binance:Url"]))
            {
                throw new ArgumentException("Binance URL is not configured in the app settings.");
            }

            if (string.IsNullOrEmpty(configuration["Polygon:Url"]))
            {
                throw new ArgumentException("Polygon URL is not configured in the app settings.");
            }

            if (string.IsNullOrEmpty(configuration["Polygon:Token"]))
            {
                throw new ArgumentException("Polygon token is not configured in the app settings.");
            }

            services
                .AddSingleton<IConfiguration>(configuration)
                .AddBinanceClient(configuration["Binance:Url"] ?? "")
                .AddPolygonClient(configuration["Polygon:Url"] ?? "", configuration["Polygon:Token"] ?? "")
                .AddSingleton<ITradingUtilities, TradingUtilities>()
                .AddScoped<ITradingHandler, TradingHandler>()
                .AddSingleton<ISimulator, Simulator>()
                .AddDatabaseServices(configuration.GetConnectionString("Database") ?? "")
                .AddHostedService<TradingService>();

            return services;
        }

        public static IServiceCollection AddDatabaseServices
            (this IServiceCollection services, string connectionString)
        {
            // Add services to the container.
            services.AddScoped<ISaveChangesInterceptor, TimestampEntityInterceptor>();

            NpgsqlDataSourceBuilder dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.UseJsonNet();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<HvtTradeDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(dataSource, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5, // Retry up to 5 times
                        maxRetryDelay: TimeSpan.FromSeconds(5), // Maximum delay between retries
                        errorCodesToAdd: null); // Use default transient error codes
                });
            });

            services.AddScoped<IHvtTradeDbContext, HvtTradeDbContext>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            return services;
        }
    }
}
