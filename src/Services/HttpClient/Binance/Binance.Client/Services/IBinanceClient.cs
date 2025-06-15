using Binance.Data.Dto;
using Binance.Data.Responses;

namespace Binance.Client.Services
{
    public interface IBinanceClient
    {
        Task<BinanceResponse<TickerPriceDto>> GetTickerPrice(string symbol, CancellationToken cancellationToken = default);
    }
}
