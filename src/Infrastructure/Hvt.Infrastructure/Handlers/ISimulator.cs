using Binance.Data.Dto;
using Hvt.Data.Dtos;
using Hvt.Infrastructure.Simulation;
using Polygon.Data.Dto;

namespace Hvt.Infrastructure.Handlers
{
    public interface ISimulator
    {
        public List<ValueDto> RsiValues { get; }
        public List<KlineDto> BinanceKlineDtos { get; }
        void Reset();
        SimulationDto? GetNext();
    }
}
