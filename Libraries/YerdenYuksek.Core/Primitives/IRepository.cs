using YerdenYuksek.Core.Caching;

namespace YerdenYuksek.Core.Primitives;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    #region Public Methods

    Task<IList<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    #endregion

    #region Properties

    IQueryable<TEntity> Table { get; }

    #endregion
}