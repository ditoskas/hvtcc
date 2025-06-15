using Hvt.Data.Dtos;
using Hvt.Data.Models;

namespace Hvt.Infrastructure.Handlers
{
    public interface ITradingHandler
    {
        void Init(bool isSimulation);
        Task CheckSymbol(Symbol symbolToCheck);
        Task CheckSymbol(Symbol symbolToCheck, SimulationDto simulationData);
        List<Symbol> SymbolsToCheck { get; set; }
        decimal TotalCapital { get; set; }
        StatisticsDto GetStatistics();
    }
}
