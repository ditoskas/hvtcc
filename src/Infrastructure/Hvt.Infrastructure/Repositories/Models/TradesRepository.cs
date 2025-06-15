using Hvt.Data.Models;
using Hvt.Infrastructure.Repositories.Contract;
using Hvt.Infrastructure.Repositories.DbContext;

namespace Hvt.Infrastructure.Repositories.Models
{
    public class TradesRepository(HvtTradeDbContext repositoryContext) : RepositoryBase<Trade, HvtTradeDbContext>(repositoryContext), ITradesRepository    
    {
    }
}
