using Hvt.Data.Models;

namespace Hvt.Infrastructure.Repositories.Contract
{
    public interface ISymbolsRepository : IRepositoryBase<Symbol>
    {
        Symbol? GetByName(string name);
        IEnumerable<Symbol> FindActiveSymbols();
    }
}
