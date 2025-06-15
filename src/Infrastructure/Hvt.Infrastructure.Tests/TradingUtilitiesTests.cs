using Binance.Client.Services;
using Hvt.Data.Dtos;
using Hvt.Data.Enums;
using Hvt.Infrastructure.Handlers;
using Polygon.Client.Services;
using Xunit;

namespace Hvt.Infrastructure.Tests
{
    public class TradingUtilitiesTests
    {
        private readonly TradingUtilities _tradingUtilities;

        public TradingUtilitiesTests()
        {
            // You may need to mock these clients if they have dependencies
            var binanceClient = new BinanceClient(new HttpClient());
            var polygonClient = new PolygonClient(new HttpClient());
            _tradingUtilities = new TradingUtilities(binanceClient, polygonClient);
        }
        [Fact]
        public void CalculateVolume_ShouldReturnValidResponse()
        {
            decimal price = 100.0m;
            decimal amount = 10000.0m;
            decimal expectedVolume = amount / price;
            decimal calculatedAmount = _tradingUtilities.CalculateVolume(price, amount);
            Assert.Equal(expectedVolume, calculatedAmount);
        }

        [Fact]
        public void CalculateTargetPrice_ShouldReturnValidResponse()
        {
            decimal currentPrice = 100000.0m;
            decimal profitPercentage = 0.3m;
            decimal lossPercentage = 0.05m;
            TargetPrices pricesOnBuyDirection = _tradingUtilities.CalculateTargetPrice(currentPrice, TradeType.Buy, profitPercentage, lossPercentage);
            TargetPrices pricesOnSellDirection = _tradingUtilities.CalculateTargetPrice(currentPrice, TradeType.Sell, profitPercentage, lossPercentage);

            decimal expectedBuyTargetPrice = currentPrice * (1 + profitPercentage);
            decimal expectedBuyStopLossPrice = currentPrice * (1 - lossPercentage);
            decimal expectedSellTargetPrice = currentPrice * (1 - profitPercentage);
            decimal expectedSellStopLossPrice = currentPrice * (1 + lossPercentage);
            Assert.Equal(expectedBuyTargetPrice, pricesOnBuyDirection.TakeProfit);
            Assert.Equal(expectedBuyStopLossPrice, pricesOnBuyDirection.StopLoss);
            Assert.Equal(expectedSellTargetPrice, pricesOnSellDirection.TakeProfit);
            Assert.Equal(expectedSellStopLossPrice, pricesOnSellDirection.StopLoss);
        }
    }
}
