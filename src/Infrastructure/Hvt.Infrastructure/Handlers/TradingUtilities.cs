using Binance.Client.Services;
using Binance.Data.Dto;
using Binance.Data.Responses;
using Hvt.Data.Dtos;
using Hvt.Data.Enums;
using Polygon.Client.Services;
using Polygon.Data.Enums;
using Polygon.Data.Responses;

namespace Hvt.Infrastructure.Handlers
{
    public class TradingUtilities(BinanceClient binanceClient, PolygonClient polygonClient) : ITradingUtilities
    {
        const decimal MaxRsi = 80m;
        const decimal MinRsi = 20m;

        public async Task<decimal> GetTickerPrice(string symbol)
        {
            BinanceResponse<TickerPriceDto> response = await binanceClient.GetTickerPrice(symbol);
            if (response is { Result: true, Data: not null })
            {
                return Convert.ToDecimal(response.Data.Price);
            }
            else
            {
                throw new Exception($"Failed to get ticker price for {symbol}: {response.Error}");
            }
        }

        public async Task<decimal> GetRsi(string polygonTicker)
        {
            PolygonResponse<RsiResponse?> response = await polygonClient.GetRsi(polygonTicker, TimespanPeriod.Minute);
            if (response is { Result: true, Data: not null })
            {
                return response.Data.Results.Values.FirstOrDefault()?.Value ??
                       throw new Exception($"No RSI value found for {polygonTicker} at the latest timestamp.");
            }
            else
            {
                throw new Exception($"Failed to get RSI for {polygonTicker}: {response.Error}");
            }
        }

        public TradeAction GetRsiAction(decimal rsi)
        {
            return rsi switch
            {
                < MinRsi => TradeAction.Buy,
                > MaxRsi => TradeAction.Sell,
                _ => TradeAction.None
            };
        }

        public async Task<RsiWithAction> GetRsiWithAction(string ticker)
        {
            decimal rsi = await GetRsi(ticker);
            TradeAction action = GetRsiAction(rsi);
            return new RsiWithAction(rsi, action);
        }

        public decimal CalculateVolume(decimal price, decimal amount)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.", nameof(price));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
            }

            return amount / price;
        }

        public TargetPrices CalculateTargetPrice(decimal currentPrice, TradeType tradeType, decimal profitPercentage,
            decimal lossPercentage)
        {
            if (currentPrice <= 0)
            {
                throw new ArgumentException("Current price must be greater than zero.", nameof(currentPrice));
            }

            if (profitPercentage <= 0)
            {
                throw new ArgumentException("Profit percentage must be greater than zero.", nameof(profitPercentage));
            }

            if (lossPercentage <= 0)
            {
                throw new ArgumentException("Loss percentage must be greater than zero.", nameof(lossPercentage));
            }

            decimal profitPriceToAdjust = (tradeType == TradeType.Buy) ? 1 + profitPercentage : 1 - profitPercentage;
            decimal lossPriceToAdjust = (tradeType == TradeType.Buy) ? 1 - lossPercentage : 1 + lossPercentage;

            decimal takeProfit = currentPrice * profitPriceToAdjust;
            decimal stopLoss = currentPrice * lossPriceToAdjust;

            return new TargetPrices(takeProfit, stopLoss);
        }

        public TradeAction ActionToExecute(decimal rsi, decimal orderPrice, decimal currentPrice, TradeType tradeType, decimal profitPercentage, decimal lossPercentage)
        {
            TradeAction rsiAction = GetRsiAction(rsi);
            if (rsiAction == TradeAction.None)
            {
                TargetPrices targetPrices = CalculateTargetPrice(orderPrice, tradeType, profitPercentage, lossPercentage);
                if (tradeType == TradeType.Buy)
                {
                    if (currentPrice >= targetPrices.TakeProfit || currentPrice <= targetPrices.StopLoss)
                    {
                        rsiAction = TradeAction.Sell;
                    }
                }
                else if (tradeType == TradeType.Sell)
                {
                    if (currentPrice <= targetPrices.TakeProfit || currentPrice >= targetPrices.StopLoss)
                    {
                        rsiAction = TradeAction.Buy;
                    }
                }
            }

            return rsiAction;
        }
    }
}
