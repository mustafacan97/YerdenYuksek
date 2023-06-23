using eCommerce.Infrastructure.Persistence.Primitives;
using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    #region Fields

    private readonly ApplicationDbContext _dbContext;

    private readonly DbSet<T> _dbSet;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructure and Destructure

    public Repository(
        ApplicationDbContext dbContext,
        IStaticCacheManager staticCacheManager)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Public Properties

    public IQueryable<T> Table => _dbSet;

    #endregion

    #region Public Methods

    public async Task<IList<T>> GetAllAsync(
        Func<IQueryable<T>, IQueryable<T>>? func = null, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true)
    {
        async Task<IList<T>> getAllAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? func(query) : query;

            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    #endregion

    #region Methods

    private IQueryable<T> AddDeletedFilter(IQueryable<T> query, in bool includeDeleted)
    {
        if (includeDeleted)
        {
            return query;
        }

        if (typeof(T).GetInterface(nameof(ISoftDeletedEntity)) == null)
        {
            return query;
        }

        return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<T>();
    }

    private async Task<IList<T>> GetEntitiesAsync(
        Func<Task<IList<T>>> getAllAsync, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null)
    {
        if (getCacheKey is null)
        {
            return await getAllAsync();
        }

        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekEntityCacheDefaults<T>.AllCacheKey);
        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
    }

    #endregion
}