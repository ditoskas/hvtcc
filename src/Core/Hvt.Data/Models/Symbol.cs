using Hvt.Data.Dtos;
using Hvt.Data.Enums;
using Hvt.Data.Models.Base;

namespace Hvt.Data.Models
{
    public class Symbol : EntityWithTimestamps
    {
        public string Name { get; set; } = string.Empty;
        public string BinanceSymbol { get; set; } = string.Empty;
        public string PolygonTicker { get; set; } = string.Empty;
        public int LeverageToTrade { get; set; }
        public decimal EquityPercentageToBet { get; set; }
        public decimal ProfitTargetPercentage { get; set; }
        public decimal StopLossTargetPercentage { get; set; }
        public bool IsActive { get; set; } = true;

        public decimal GetAmountToInvest(decimal totalCapital)
        {
            return totalCapital * EquityPercentageToBet;
        }

        public TargetPrices CalculateTargetPrice(decimal currentPrice, TradeType tradeType)
        {
            if (currentPrice <= 0)
            {
                throw new ArgumentException("Current price must be greater than zero.", nameof(currentPrice));
            }

            decimal profitPriceToAdjust = (tradeType == TradeType.Buy) ? 1 + ProfitTargetPercentage : 1 - ProfitTargetPercentage;
            decimal lossPriceToAdjust = (tradeType == TradeType.Buy) ? 1 - StopLossTargetPercentage : 1 + StopLossTargetPercentage;

            decimal takeProfit = currentPrice * profitPriceToAdjust;
            decimal stopLoss = currentPrice * lossPriceToAdjust;

            return new TargetPrices(takeProfit, stopLoss);
        }
    }
}
