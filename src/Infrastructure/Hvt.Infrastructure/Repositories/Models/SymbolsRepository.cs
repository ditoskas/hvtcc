using Hvt.Data.Models;
using Hvt.Infrastructure.Repositories.Contract;
using Hvt.Infrastructure.Repositories.DbContext;

namespace Hvt.Infrastructure.Repositories.Models
{
    public class SymbolsRepository(HvtTradeDbContext repositoryContext) : RepositoryBase<Symbol, HvtTradeDbContext>(repositoryContext), ISymbolsRepository
    {
        public Symbol? GetByName(string name)
        {
            return FindByCondition(s => s.Name == name, true).FirstOrDefault();
        }

        public IEnumerable<Symbol> FindActiveSymbols()
        {
            return FindByCondition(s => s.IsActive == true, true);
        }
    }
}
