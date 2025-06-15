using Hvt.Data.Dtos;
using Hvt.Data.Enums;
using Hvt.Data.Models;
using Hvt.Infrastructure.Repositories.Contract;
using Hvt.Utilities;
using Microsoft.Extensions.Logging;

namespace Hvt.Infrastructure.Handlers
{
    public class TradingHandler (ITradingUtilities tradingUtilities, IRepositoryManager repositoryManager, ILogger<TradingHandler> logger) : ITradingHandler
    {
        readonly TradeCollection _tradeCollection = new TradeCollection();
        readonly IList<Trade> _closedTradeCollection = new List<Trade>(); //used only on simulation

        public List<Symbol> SymbolsToCheck { get; set; } = new List<Symbol>();
        public decimal TotalCapital { get; set; }
        private bool _isSimulation = true;

        public void Init(bool isSimulation)
        {
            _isSimulation = isSimulation;
            logger.LogInformation($"Initialize Trading Handler, Simulation: {isSimulation}");
            SymbolsToCheck = repositoryManager.Symbols.FindActiveSymbols().ToList();
            TotalCapital = isSimulation ? 30000000 : repositoryManager.AppSettings.GetCurrentCapital();
            logger.LogInformation($"Total Symbols To Check: {SymbolsToCheck.Count}");
            logger.LogInformation($"Current Capital: {TotalCapital}");
        }

        public StatisticsDto GetStatistics()
        {
            StatisticsDto statistics = new StatisticsDto
            {
                TotalTrades = _closedTradeCollection.Count,
                TotalProfitAndLoss = _closedTradeCollection.Sum(trade => trade.ProfitAndLoss ?? 0),
                TotalTradesInProfit = _closedTradeCollection.Count(trade => trade.ProfitAndLoss >= 0),
                TotalTradesInLoss = _closedTradeCollection.Count(trade => trade.ProfitAndLoss < 0)
            };
            return statistics;
        }

        public async Task CheckSymbol(Symbol symbolToCheck, SimulationDto simulationData)
        {
            DateTime date = DateHelper.ConvertTimestampToDateTime(simulationData.Rsi.Timestamp);
            logger.LogWarning($"[{date}][{simulationData.Rsi.Timestamp}] Simulating symbol: {symbolToCheck.Name}");
            try
            {
                decimal currentPrice = simulationData.Kline.GetMediumPrice();
                RsiWithAction rsiWithAction = new RsiWithAction(simulationData.Rsi.Value, tradingUtilities.GetRsiAction(simulationData.Rsi.Value));
                logger.LogInformation($"RSI for {symbolToCheck.Name}: {rsiWithAction.Rsi}, Action: {rsiWithAction.Action}, Current Price: [{currentPrice}]");
                Trade? openTrade = _tradeCollection.GetTrade(symbolToCheck.Name);
                bool hasOpenTrade = openTrade != null;
                logger.LogInformation($"Has open trade for {symbolToCheck.Name}: {hasOpenTrade}");
                await Process(rsiWithAction, hasOpenTrade, openTrade, symbolToCheck, currentPrice);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error checking symbol {symbolToCheck.Name} in simulation");
            }
        }

        public async Task CheckSymbol(Symbol symbolToCheck)
        {
            logger.LogWarning($"[{DateTime.UtcNow}] Checking symbol: {symbolToCheck.Name}");
            try
            {
                //Get RSI
                RsiWithAction rsiWithAction = await tradingUtilities.GetRsiWithAction(symbolToCheck.PolygonTicker);
                decimal currentPrice = await tradingUtilities.GetTickerPrice(symbolToCheck.BinanceSymbol);
                logger.LogInformation($"RSI for {symbolToCheck.Name}: {rsiWithAction.Rsi}, Action: {rsiWithAction.Action}");
                Trade? openTrade = _tradeCollection.GetTrade(symbolToCheck.Name);
                bool hasOpenTrade = openTrade != null;
                logger.LogInformation($"Has open trade for {symbolToCheck.Name}: {hasOpenTrade}");
                await Process(rsiWithAction, hasOpenTrade, openTrade, symbolToCheck, currentPrice);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error checking symbol {symbolToCheck.Name}");
                return;
            }
            
        }

        protected async Task Process(RsiWithAction rsiWithAction, bool hasOpenTrade, Trade? openTrade, Symbol symbolToCheck, decimal currentPrice)
        {
            if (rsiWithAction.Action == TradeAction.None && hasOpenTrade)
            {
                //Check if profit target or stop loss is reached
                TradeAction actionToExecute = tradingUtilities.ActionToExecute(rsiWithAction.Rsi, openTrade.OpenPrice, currentPrice, openTrade.TradeType,
                    symbolToCheck.ProfitTargetPercentage, symbolToCheck.StopLossTargetPercentage);
                if (openTrade.TradeType == TradeType.Buy && actionToExecute == TradeAction.Sell ||
                    openTrade.TradeType == TradeType.Sell && actionToExecute == TradeAction.Buy)
                {
                    openTrade.CloseAt = DateTime.UtcNow;
                    openTrade.ClosePrice = currentPrice;
                    openTrade.UpdateProfitAndLoss(currentPrice);
                    TotalCapital += openTrade.InvestedAmount + (openTrade.ProfitAndLoss ?? 0);
                    logger.LogCritical($"Profit Split or Stop Loss for [{symbolToCheck.Name}] reached: openTrade.TradeType[{openTrade.TradeType}] - actionToExecute[{actionToExecute}] - Profit[{openTrade.ProfitAndLoss}]");
                    logger.LogDebug($"New Capital: {TotalCapital}");
                    //Close trade
                    if (_tradeCollection.RemoveTrade(symbolToCheck.Name))
                    {
                        if (!_isSimulation)
                        {
                            repositoryManager.AppSettings.SetCurrentCapital(TotalCapital);
                            repositoryManager.Trades.Update(openTrade);
                            await repositoryManager.SaveChangesAsync();
                        }
                        else
                        {
                            _closedTradeCollection.Add(openTrade);
                        }
                    }
                    else
                    {
                        logger.LogWarning($"Failed to remove trade for {symbolToCheck.Name}.");
                    }
                }
            }
            else if (hasOpenTrade)
            {
                //RSI reversal action
                if (openTrade.TradeType == TradeType.Buy && rsiWithAction.Action == TradeAction.Sell ||
                    openTrade.TradeType == TradeType.Sell && rsiWithAction.Action == TradeAction.Buy)
                {
                    openTrade.CloseAt = DateTime.UtcNow;
                    openTrade.ClosePrice = currentPrice;
                    openTrade.UpdateProfitAndLoss(currentPrice);
                    TotalCapital += openTrade.InvestedAmount + (openTrade.ProfitAndLoss ?? 0);
                    logger.LogCritical($"RSI Action for [{symbolToCheck.Name}] reached: openTrade.TradeType[{openTrade.TradeType}] - rsiWithAction.Action[{rsiWithAction.Action}][{rsiWithAction.Rsi}] - Profit[{openTrade.ProfitAndLoss}]");
                    logger.LogDebug($"New Capital: {TotalCapital}");
                    //Close trade
                    if (_tradeCollection.RemoveTrade(symbolToCheck.Name))
                    {
                        if (!_isSimulation)
                        {
                            repositoryManager.AppSettings.SetCurrentCapital(TotalCapital);
                            repositoryManager.Trades.Update(openTrade);
                            await repositoryManager.SaveChangesAsync();
                        }
                        else
                        {
                            _closedTradeCollection.Add(openTrade);
                        }
                    }
                    else
                    {
                        logger.LogWarning($"Failed to remove trade for {symbolToCheck.Name}.");
                    }
                }
                else
                {
                    logger.LogInformation($"Open Trade Exists [{symbolToCheck.Name}] No action taken");
                }
            }
            else if (rsiWithAction.Action != TradeAction.None)
            {
                //Open new trade based on the RSI action
                decimal amountToInvestOnTrade = symbolToCheck.GetAmountToInvest(TotalCapital);
                if (amountToInvestOnTrade <= 0)
                {
                    logger.LogWarning($"Amount to invest for {symbolToCheck.Name} is zero or negative. Skipping trade.");
                    return;
                }
                decimal volume = tradingUtilities.CalculateVolume(currentPrice, amountToInvestOnTrade);
                Trade newTrade = new Trade()
                {
                    Symbol = symbolToCheck,
                    SymbolId = symbolToCheck.Id,
                    OpenPrice = currentPrice,
                    TradeType = rsiWithAction.Action == TradeAction.Buy ? TradeType.Buy : TradeType.Sell,
                    InvestedAmount = amountToInvestOnTrade,
                    Volume = volume,
                    Leverage = symbolToCheck.LeverageToTrade,
                };
                if (_tradeCollection.AddTrade(newTrade))
                {
                    TotalCapital -= amountToInvestOnTrade;
                    logger.LogCritical($"Open Trade\r\n{newTrade}");
                    logger.LogDebug($"New Capital: {TotalCapital}.");
                    if (!_isSimulation)
                    {
                        repositoryManager.AppSettings.SetCurrentCapital(TotalCapital);
                        repositoryManager.Trades.Create(newTrade);
                        await repositoryManager.SaveChangesAsync();
                    }

                }
                else
                {
                    logger.LogWarning($"Failed to add trade for {symbolToCheck.Name}.");
                }
            }
        }

       
    }
}
