using Binance.Data.Dto;
using Polygon.Data.Dto;

namespace Hvt.Data.Dtos
{
    public record SimulationDto(ValueDto Rsi, KlineDto Kline);
}
