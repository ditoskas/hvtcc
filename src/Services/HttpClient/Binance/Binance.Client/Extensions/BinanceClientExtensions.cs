using BaseClient.Extensions;
using Binance.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Binance.Client.Extensions
{
    public static class BinanceClientExtensions
    {
        public static IServiceCollection AddBinanceClient(this IServiceCollection services, string baseUrl)
        {
            services.AddBaseClient<BinanceClient>(baseUrl);
            return services;
        }
    }
}
