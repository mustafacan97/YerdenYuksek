using System.Linq.Expressions;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Shared;

namespace eCommerce.Core.Interfaces;

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

    Task<IPagedInfo<T>> GetAllPagedAsync(
        Func<IQueryable<T>, IQueryable<T>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true);

    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool onlyActive = true,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null);

    T? GetById(Guid id, bool onlyActive = true);

    Task<T?> GetByIdAsync(Guid id, bool onlyActive = true);

    Task<IList<T>> GetByIdsAsync(
        IList<Guid> ids,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true);

    Task InsertAsync(T entity);

    void Insert(T entity);

    Task InsertAsync(IList<T> entities);

    void Insert(IList<T> entities);

    void Update(T entity);

    void Update(IList<T> entities);

    void Delete(T entity);

    void Truncate();

    #endregion

    #region Properties

    IQueryable<T> Table { get; }

    #endregion
}