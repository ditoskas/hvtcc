using Hvt.Infrastructure.Repositories.Contract;
using Hvt.Infrastructure.Repositories.DbContext;
using Hvt.Infrastructure.Repositories.Models;

namespace Hvt.Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly HvtTradeDbContext _dbContext;

        #region Private Repos
        private readonly Lazy<IAppSettingsRepository> _appSettings;
        private readonly Lazy<ITradesRepository> _trades;
        private readonly Lazy<ISymbolsRepository> _symbols;
        #endregion

        public RepositoryManager(HvtTradeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _appSettings = new Lazy<IAppSettingsRepository>(() => new AppSettingsRepository(_dbContext));
            _trades = new Lazy<ITradesRepository>(() => new TradesRepository(_dbContext));
            _symbols = new Lazy<ISymbolsRepository>(() => new SymbolsRepository(_dbContext));
        }

        #region Public Repos
        public IAppSettingsRepository AppSettings => _appSettings.Value;
        public ITradesRepository Trades => _trades.Value;
        public ISymbolsRepository Symbols => _symbols.Value;
        #endregion

        #region Public Methods
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);
        public int SaveChanges() => _dbContext.SaveChanges();
        #endregion

    }
}
