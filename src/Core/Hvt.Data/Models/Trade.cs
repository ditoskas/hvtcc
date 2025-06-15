using Hvt.Data.Enums;
using Hvt.Data.Models.Base;
using System.Text.Json;

namespace Hvt.Data.Models
{
    public class Trade : EntityWithTimestamps
    {
        public long SymbolId { get; set; }
        public Symbol Symbol { get; set; } = null!;
        public TradeType TradeType { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal? ClosePrice { get; set; }
        public decimal? ProfitAndLoss { get; set; }
        public decimal Volume { get; set; }
        public decimal InvestedAmount { get; set; }
        public int Leverage { get; set; }
        public DateTime OpenAt { get; set; }
        public DateTime? CloseAt { get; set; }

        public decimal CalculateProfitAndLoss(decimal currentPrice)
        {
            if (TradeType == TradeType.Buy)
            {
                return (currentPrice - OpenPrice) * Volume * Leverage;
            }
            else if (TradeType == TradeType.Sell)
            {
                return (OpenPrice - currentPrice) * Volume * Leverage;
            }
            return 0;
        }
        public void UpdateProfitAndLoss(decimal currentPrice)
        {
            ProfitAndLoss = CalculateProfitAndLoss(currentPrice);
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
