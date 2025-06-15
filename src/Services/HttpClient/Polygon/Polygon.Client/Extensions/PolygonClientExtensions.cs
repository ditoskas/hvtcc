using BaseClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Polygon.Client.Services;

namespace Polygon.Client.Extensions
{
    public static class PolygonClientExtensions
    {
        public static IServiceCollection AddPolygonClient(this IServiceCollection services, string baseUrl, string token)
        {
            services.AddBaseClient<PolygonClient>(baseUrl, token);
            return services;
        }

    }
}
