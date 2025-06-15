using Polygon.Data.Responses;

namespace Polygon.Client.Services
{
    public interface IPolygonClient
    {
        Task<PolygonResponse<RsiResponse?>> GetRsi(string ticker, string timespan,
            string? timestamp = null, CancellationToken cancellationToken = default);
    }
}
