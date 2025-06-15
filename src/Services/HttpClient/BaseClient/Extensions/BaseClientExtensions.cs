using Microsoft.Extensions.DependencyInjection;
namespace BaseClient.Extensions
{
    public static class BaseClientExtensions
    {
        public static void AddBaseClient<T>(this IServiceCollection services, string baseUrl, string? token = null) where T : BaseClient
        {
            services.AddHttpClient<T>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(token))
                {
                    //hacky way to pass the parameter on the client
                    httpClient.DefaultRequestHeaders.Add("X-Token", token);
                }
            });
        }
    }
}
