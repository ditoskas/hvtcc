using Binance.Data.Dto;
using Binance.Data.Responses;

namespace Binance.Client.Services
{
    public class BinanceClient(HttpClient client) : BaseClient.BaseClient(client), IBinanceClient
    {
        public async Task<BinanceResponse<TickerPriceDto>> GetTickerPrice(string symbol,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentException("Symbol cannot be null or empty", nameof(symbol));
            }

            string endpoint = EndPoints.TickerPrice.Replace("{symbol}", symbol.ToUpperInvariant());
            try
            {
                TickerPriceDto? response = await Execute<TickerPriceDto>(endpoint, HttpMethod.Get, cancellationToken: cancellationToken);
                if (response == null)
                {
                    return new BinanceResponse<TickerPriceDto>
                    {
                        Result = false,
                        Error = "No data returned from the API",
                        Data = null
                    };
                }
                return new BinanceResponse<TickerPriceDto>
                {
                    Result = true,
                    Error = null,
                    Data = response
                };
            }
            catch(Exception ex)
            {
                // Log the exception or handle it as needed
                return new BinanceResponse<TickerPriceDto>
                {
                    Result = false,
                    Error = ex.Message,
                    Data = null
                };
            }
        }
    }
}
