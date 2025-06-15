using Microsoft.EntityFrameworkCore;

namespace Hvt.Infrastructure.Repositories.Contract
{
    public interface IRepositoryManager
    {
        public IAppSettingsRepository AppSettings{ get; }
        public ITradesRepository Trades { get; }
        public ISymbolsRepository Symbols { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
