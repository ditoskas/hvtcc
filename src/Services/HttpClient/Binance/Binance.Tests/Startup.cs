using Binance.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Binance.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBinanceClient("https://fapi.binance.com");
        }
    }
}
