using eCommerce.Infrastructure.Persistence.Primitives;
using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Primitives;
using YerdenYuksek.Web.Framework.Persistence.Extensions;

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

    public IList<T> GetAll(
        Func<IQueryable<T>, IQueryable<T>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true)
    {
        IList<T> getAll()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? func(query) : query;

            return query.ToList();
        }

        return GetEntities(getAll, getCacheKey);
    }

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

    public async Task<IList<T>> GetAllAsync(
        Func<IQueryable<T>, Task<IQueryable<T>>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true)
    {
        async Task<IList<T>> getAllAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? await func(query) : query;

            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public async Task<IPagedList<T>> GetAllPagedAsync(
        Func<IQueryable<T>, IQueryable<T>>? func = null,
        int pageIndex = 0, 
        int pageSize = int.MaxValue, 
        bool getOnlyTotalCount = false, 
        bool includeDeleted = true)
    {
        var query = AddDeletedFilter(Table, includeDeleted);

        query = func != null ? func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<T> GetByIdAsync(
        Guid id, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true)
    {
        async Task<T> getEntityAsync()
        {
            return await AddDeletedFilter(Table, includeDeleted).FirstOrDefaultAsync(q => q.Id == id);
        }

        if (getCacheKey is null)
        {
            return await getEntityAsync();
        }

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
            ?? _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekEntityCacheDefaults<T>.ByIdCacheKey, id);

        return await _staticCacheManager.GetAsync(cacheKey, getEntityAsync);
    }

    public T GetById(
        Guid id, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true)
    {
        T getEntity()
        {
            return AddDeletedFilter(Table, includeDeleted).FirstOrDefault(q => q.Id == id);
        }

        if (getCacheKey is null)
        {
            return getEntity();
        }

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekEntityCacheDefaults<T>.ByIdCacheKey, id);

        return _staticCacheManager.Get(cacheKey, getEntity);
    }

    public async Task<IList<T>> GetByIdsAsync(
        IList<Guid> ids, 
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true)
    {
        if (!ids.Any())
        {
            return new List<T>();
        }

        async Task<IList<T>> getByIdsAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);

            //get entries
            var entriesById = await query
                .Where(entry => ids.Contains(entry.Id))
                .ToDictionaryAsync(entry => entry.Id);

            //sort by passed identifiers
            var sortedEntries = new List<T>();
            foreach (var id in ids)
            {
                if (entriesById.TryGetValue(id, out var sortedEntry))
                {
                    sortedEntries.Add(sortedEntry);
                }
            }

            return sortedEntries;
        }

        if (getCacheKey is null)
        {
            return await getByIdsAsync();
        }

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
            ?? _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekEntityCacheDefaults<T>.ByIdsCacheKey, ids);

        return await _staticCacheManager.GetAsync(cacheKey, getByIdsAsync);
    }

    #region Insert

    public async Task InsertAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbSet.AddAsync(entity);
    }

    public void Insert(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Add(entity);
    }

    public async Task InsertAsync(IList<T> entities)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await _dbSet.AddRangeAsync(entities);
    }

    public void Insert(IList<T> entities)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        _dbSet.AddRange(entities);
    }

    #endregion

    public void Update(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Attach(entity).State = EntityState.Modified;
    }

    public void Update(IList<T> entities)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        if (entities.Count == 0)
        {
            return;
        }

        _dbSet.UpdateRange(entities);
    }

    public void Delete(T entity)
    {
        switch (entity)
        {
            case null:
                throw new ArgumentNullException(nameof(entity));

            case ISoftDeletedEntity softDeletedEntity:
                softDeletedEntity.Deleted = true;
                _dbSet.Attach(entity).State = EntityState.Modified;
                break;

            default:
                _dbSet.Attach(entity).State = EntityState.Deleted;
                break;
        }
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

    private IList<T> GetEntities(Func<IList<T>> getAll, Func<IStaticCacheManager, CacheKey> getCacheKey)
    {
        if (getCacheKey == null)
        {
            return getAll();
        }

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekEntityCacheDefaults<T>.AllCacheKey);

        return _staticCacheManager.Get(cacheKey, getAll);
    }

    #endregion
}