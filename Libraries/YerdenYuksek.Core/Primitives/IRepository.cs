using YerdenYuksek.Core.Caching;

namespace YerdenYuksek.Core.Primitives;

public interface IRepository<T> where T : BaseEntity
{
    #region Public Methods

    IList<T> GetAll(
        Func<IQueryable<T>, IQueryable<T>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    Task<IList<T>> GetAllAsync(
        Func<IQueryable<T>, IQueryable<T>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    Task<IList<T>> GetAllAsync(
        Func<IQueryable<T>, Task<IQueryable<T>>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    Task<IPagedList<T>> GetAllPagedAsync(
        Func<IQueryable<T>, IQueryable<T>>? func = null,
        int pageIndex = 0, 
        int pageSize = int.MaxValue, 
        bool getOnlyTotalCount = false, 
        bool includeDeleted = true);

    Task<T> GetByIdAsync(
        Guid id, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    T GetById(
        Guid id,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true);

    Task<IList<T>> GetByIdsAsync(
        IList<Guid> ids, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    Task InsertAsync(T entity);

    void Insert(T entity);

    Task InsertAsync(IList<T> entities);

    void Insert(IList<T> entities);

    #endregion

    #region Properties

    IQueryable<T> Table { get; }

    #endregion
}