using Microsoft.Extensions.DependencyInjection;
using Polygon.Client.Extensions;

namespace Polygon.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPolygonClient("https://api.polygon.io", "5sBcszMPgsCP0RwHjiy_DcY_tbciMsdx");
        }
    }
}
