using Hvt.Data.Dtos;
using Hvt.Data.Enums;

namespace Hvt.Infrastructure.Handlers
{
    public interface ITradingUtilities
    {
        Task<decimal> GetTickerPrice(string symbol);
        Task<decimal> GetRsi(string polygonTicker);
        TradeAction GetRsiAction(decimal rsi);
        Task<RsiWithAction> GetRsiWithAction(string ticker);
        decimal CalculateVolume(decimal price, decimal amount);
        TargetPrices CalculateTargetPrice(decimal costPrice, TradeType tradeType, decimal profitPercentage, decimal lossPercentage);
        TradeAction ActionToExecute(decimal rsi, decimal orderPrice, decimal currentPrice, TradeType tradeType, decimal profitPercentage, decimal lossPercentage);
    }
}
