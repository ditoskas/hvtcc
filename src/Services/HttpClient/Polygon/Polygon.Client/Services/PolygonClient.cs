using System.Net.Http.Headers;
using System.Text;
using Hvt.Utilities;
using Polygon.Client.Exceptions;
using Polygon.Data.Enums;
using Polygon.Data.Responses;

namespace Polygon.Client.Services
{
    public sealed class PolygonClient(HttpClient client) : BaseClient.BaseClient(client), IPolygonClient
    {
        #region Technical Indicators
        public async Task<PolygonResponse<RsiResponse?>> GetRsi(string ticker, string timespan, string? timestamp = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ticker)) throw new ArgumentNullException(nameof(ticker));
            if (string.IsNullOrEmpty(timespan) || !TimespanPeriod.IsValid(timespan)) throw new ArgumentNullException(nameof(timespan));


            string endpoint = EndPoints.RelativeStrengthIndex.Replace("{ticker}", ticker);
            endpoint += $"?timespan={timespan}&adjusted=true&window=14&series_type=close&limit=1";
            //Attention if timestamp is null, the API will return the latest RSI value otherwise it will return the RSI value at the specified timestamp.
            if (!string.IsNullOrEmpty(timestamp))
            {
                endpoint += $"&timestamp.lte={timestamp}&order=desc";
            }
            else
            {
                endpoint += "&order=desc";
            }

            try
            {
                RsiResponse? response = await Execute<RsiResponse>(endpoint, HttpMethod.Get, cancellationToken: cancellationToken);
                if (response == null)
                {
                    return new PolygonResponse<RsiResponse?>()
                    {
                        Result = false,
                        Data = null,
                        Error = "No data returned from the API."
                    };
                }
                else
                {
                    return new PolygonResponse<RsiResponse?>()
                    {
                        Result = response.Status == PolygonResponseStatus.Ok,
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new PolygonResponse<RsiResponse?>()
                {
                    Result = false,
                    Error = ex.Message,
                    Data = null
                };
            }
        }
        #endregion
    }
}
