using Hvt.Data.Dtos;
using Hvt.Data.Models;
using Hvt.Infrastructure.Handlers;
using Hvt.Infrastructure.Repositories.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hvt.Infrastructure.HostedServices
{
    public class TradingService (ILogger<TradingService> logger, IServiceProvider serviceProvider, IConfiguration configuration, ISimulator simulator) : BackgroundService
    {
        private int _delayTime = 60000;
        private ITradingHandler? _tradingHandler;
        private IRepositoryManager? _repositoryManager;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                _tradingHandler = scope.ServiceProvider.GetRequiredService<ITradingHandler>();
                _repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                logger.LogInformation("TradingService is executing.");
                bool isSimulation = configuration.GetValue<bool>("IsSimulation");
                _delayTime = isSimulation ? 1000 : 60000;
                _tradingHandler.Init(isSimulation);
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (isSimulation)
                    {
                        SimulationProcess();
                    }
                    else
                    {
                        RealProcess();
                    }
                    await Task.Delay(_delayTime, stoppingToken);
                }
            }
            
        }

        protected void SimulationProcess()
        {
            Symbol? symbolToCheck = _repositoryManager.Symbols.GetByName("BTCUSDT");
            SimulationDto? nextSimulation = simulator.GetNext();
            while (nextSimulation != null)
            {
                _tradingHandler.CheckSymbol(symbolToCheck, nextSimulation).GetAwaiter().GetResult();
                nextSimulation = simulator.GetNext();
                Task.Delay(_delayTime);
            }
            logger.LogInformation("Simulation completed.");
            StatisticsDto stats = _tradingHandler.GetStatistics();
            logger.LogCritical($"Simulation statistics: " +
                                  $"Total Trades: {stats.TotalTrades}, " +
                                  $"Total Profit: {stats.TotalProfitAndLoss}, " +
                                  $"Total Trades in Profit: {stats.TotalTradesInProfit}, " +
                                  $"Total Trades in Loss: {stats.TotalTradesInLoss}");
            _delayTime = 60 * 60000; //Set delay time to hour 
        }

        protected void RealProcess()
        {
            _tradingHandler.SymbolsToCheck.ForEach(symbol =>
            {
                try
                {
                    _tradingHandler.CheckSymbol(symbol).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error checking symbol {symbol.Name}");
                }
            });
        }
    }
}
