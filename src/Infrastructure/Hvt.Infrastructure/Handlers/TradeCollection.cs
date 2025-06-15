using Hvt.Data.Models;
using System.Collections.Concurrent;

namespace Hvt.Infrastructure.Handlers
{
    public class TradeCollection
    {
        private readonly ConcurrentDictionary<string, Trade> _trades = new ConcurrentDictionary<string, Trade>();
        public bool AddTrade(Trade trade)
        {
            if (trade == null) throw new ArgumentNullException(nameof(trade));
            return _trades.TryAdd(trade.Symbol.Name, trade);
        }
        public bool RemoveTrade(string symbol)
        {
            if (string.IsNullOrEmpty(symbol)) throw new ArgumentException("Symbol cannot be null.", nameof(symbol));
            return _trades.TryRemove(symbol, out _);
        }

        public Trade? GetTrade(string symbol)
        {
            if (string.IsNullOrEmpty(symbol)) throw new ArgumentException("Symbol cannot be null.", nameof(symbol));
            _trades.TryGetValue(symbol, out Trade? trade);
            return trade;
        }
    }
}
