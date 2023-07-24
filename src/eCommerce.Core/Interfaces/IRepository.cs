using System.Linq.Expressions;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Shared;

namespace eCommerce.Core.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    #region Properties

    IQueryable<TEntity> Table { get; }

    #endregion

    #region Public Methods

    Task<TEntity?> GetByIdAsync(Guid id, Func<IStaticCacheManager, CacheKey>? getCacheKey = null, bool includeDeleted = true);

    TEntity? GetById(Guid id, Func<IStaticCacheManager, CacheKey>? getCacheKey = null, bool includeDeleted = true);

    Task<IList<TEntity>> GetByIdsAsync(IList<Guid> ids, Func<IStaticCacheManager, CacheKey>? getCacheKey = null, bool includeDeleted = true);

    Task<IList<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    Task<IList<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    IList<TEntity> GetAll(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true);

    Task<IList<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func,
        Func<IStaticCacheManager, Task<CacheKey>> getCacheKey, 
        bool includeDeleted = true);

    Task<IPagedInfo<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        int pageIndex = 0, 
        int pageSize = int.MaxValue, 
        bool getOnlyTotalCount = false, 
        bool includeDeleted = true);

    Task<IPagedInfo<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0, 
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true);

    Task InsertAsync(TEntity entity, bool publishEvent = true);

    void Insert(TEntity entity, bool publishEvent = true);

    Task InsertAsync(IList<TEntity> entities, bool publishEvent = true);

    void Insert(IList<TEntity> entities, bool publishEvent = true);

    Task UpdateAsync(TEntity entity, bool publishEvent = true);

    void Update(TEntity entity, bool publishEvent = true);

    Task UpdateAsync(IList<TEntity> entities, bool publishEvent = true);

    void Update(IList<TEntity> entities, bool publishEvent = true);

    Task DeleteAsync(TEntity entity, bool publishEvent = true);

    void Delete(TEntity entity, bool publishEvent = true);

    Task DeleteAsync(IList<TEntity> entities, bool publishEvent = true);

    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    int Delete(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> LoadOriginalCopyAsync(TEntity entity);

    Task TruncateAsync(bool resetIdentity = false);

    #endregion    
}