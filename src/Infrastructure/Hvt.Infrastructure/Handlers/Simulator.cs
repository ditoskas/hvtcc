using Binance.Data.Dto;
using Hvt.Data.Dtos;
using Hvt.Infrastructure.Simulation;
using Hvt.Utilities;
using Polygon.Data.Dto;
using Polygon.Data.Responses;

namespace Hvt.Infrastructure.Handlers
{
    public class Simulator : ISimulator
    {
        public int CurrentIndex = 0;

        public Simulator()
        {
            RsiValues = SimulationData.GetPolygonRsiValues();
            BinanceKlineDtos = SimulationData.GetBinanceKlineDtos();
        }

        public List<ValueDto> RsiValues { get; }
        public List<KlineDto> BinanceKlineDtos { get; }

        public void Reset()
        {
            CurrentIndex = 0;
        }

        public SimulationDto? GetNext()
        {
            if (CurrentIndex >= RsiValues.Count || CurrentIndex >= BinanceKlineDtos.Count)
            {
                return null;
            }

            ValueDto rsi = RsiValues[CurrentIndex];
            KlineDto kline = BinanceKlineDtos[CurrentIndex];
            CurrentIndex++;
            return new SimulationDto(rsi, kline);

        }
    }
}
