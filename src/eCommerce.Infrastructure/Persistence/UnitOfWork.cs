using eCommerce.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence;

public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
{
    #region Fields

    private readonly TContext _dbContext;

    private bool _disposed;

    private Dictionary<Type, object> repositories;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructors and Destructors

    public UnitOfWork(TContext dbContext, IStaticCacheManager staticCacheManager)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Public Methods

    public TContext DbContext => _dbContext;

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        var count = 0;
        foreach (var unitOfWork in unitOfWorks)
        {
            count += await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        count += await SaveChangesAsync();

        ts.Complete();

        return count;
    }

    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        repositories ??= new Dictionary<Type, object>();

        var type = typeof(T);
        if (!repositories.ContainsKey(type))
        {
            repositories[type] = new Repository<T>(_dbContext, _staticCacheManager);
        }

        return (IRepository<T>)repositories[type];
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    #endregion
}
