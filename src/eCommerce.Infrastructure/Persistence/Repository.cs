using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Shared;
using eCommerce.Infrastructure.Extensions;
using eCommerce.Infrastructure.Persistence.DataProviders;
using System.Linq.Expressions;
using System.Text.Json;
using System.Transactions;

namespace eCommerce.Infrastructure.Persistence;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    private readonly ICustomDataProvider _dataProvider;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructure and Destructure

    public Repository(
        ICustomDataProvider dataProvider,
        IStaticCacheManager staticCacheManager)
    {
        _dataProvider = dataProvider;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Public Properties

    public IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>();

    #endregion

    #region Public Methods

    public async Task<TEntity?> GetByIdAsync(Guid id, Func<IStaticCacheManager, CacheKey>? getCacheKey = null, bool includeDeleted = true)
    {
        async Task<TEntity?> getEntityAsync()
        {
            return await AddDeletedFilter(Table, includeDeleted).FirstOrDefaultAsync(entity => entity.Id == id);
        }

        if (getCacheKey is null)
        {
            return await getEntityAsync();
        }

        var cacheKey = getCacheKey(_staticCacheManager)
            ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdCacheKey, id);

        return await _staticCacheManager.GetAsync(cacheKey, getEntityAsync);
    }

    public TEntity? GetById(Guid id, Func<IStaticCacheManager, CacheKey>? getCacheKey = null, bool includeDeleted = true)
    {
        TEntity? getEntity()
        {
            return AddDeletedFilter(Table, includeDeleted).FirstOrDefault(entity => entity.Id == id);
        }

        if (getCacheKey is null)
        {
            return getEntity();
        }

        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdCacheKey, id);

        return _staticCacheManager.Get(cacheKey, getEntity);
    }

    public async Task<IList<TEntity>> GetByIdsAsync(
        IList<Guid> ids,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true)
    {
        if (!ids.Any())
        {
            return new List<TEntity>();
        }

        async Task<IList<TEntity>> getByIdsAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);

            var entriesById = await query
                .Where(entry => ids.Contains(entry.Id))
                .ToDictionaryAsync(entry => entry.Id);

            var sortedEntries = new List<TEntity>();
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

        var cacheKey = getCacheKey(_staticCacheManager)
            ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);

        return await _staticCacheManager.GetAsync(cacheKey, getByIdsAsync);
    }

    public IList<TEntity> GetAll(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true)
    {
        IList<TEntity> getAll()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? func(query) : query;

            return query.ToList();
        }

        if (getCacheKey is null)
        {
            return getAll();
        }

        return GetEntities(getAll, getCacheKey);
    }

    public async Task<IList<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? func(query) : query;

            return await query.ToListAsync();
        }

        if (getCacheKey is null)
        {
            return await getAllAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public async Task<IList<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func,
        Func<IStaticCacheManager, CacheKey> getCacheKey,
        bool includeDeleted = true)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? await func(query) : query;

            return await query.ToListAsync();
        }

        if (getCacheKey is null)
        {
            return await getAllAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public async Task<IPagedInfo<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true)
    {
        var query = AddDeletedFilter(Table, includeDeleted);

        query = func != null ? func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<IPagedInfo<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true)
    {
        var query = AddDeletedFilter(Table, includeDeleted);

        query = func != null ? await func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public TEntity? GetFirstOrDefault(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null,
        bool includeDeleted = true)
    {
        return GetFirstOrDefaultAsync(func, getCacheKey, includeDeleted).GetAwaiter().GetResult();
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null, 
        bool includeDeleted = true)
    {
        async Task<TEntity?> getEntityAsync() => await AddDeletedFilter(func is null ? Table : func(Table), includeDeleted).FirstOrDefaultAsync();

        if (getCacheKey is null)
        {
            return await getEntityAsync();
        }

        var cacheKey = getCacheKey(_staticCacheManager);

        return await _staticCacheManager.GetAsync(cacheKey, getEntityAsync);
    }

    #region Insert

    public async Task InsertAsync(TEntity entity, bool publishEvent = true)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _dataProvider.InsertEntityAsync(entity);

        if (publishEvent) PublishEvents(entity);

        transaction.Complete();
    }

    public void Insert(TEntity entity, bool publishEvent = true)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        using var transaction = new TransactionScope();
        _dataProvider.InsertEntity(entity);

        if (publishEvent) PublishEvents(entity);

        transaction.Complete();
    }

    public async Task InsertAsync(IList<TEntity> entities, bool publishEvent = true)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _dataProvider.BulkInsertEntitiesAsync(entities);

        if (publishEvent) PublishEvents(entities.ToArray());

        transaction.Complete();
    }

    public void Insert(IList<TEntity> entities, bool publishEvent = true)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        using var transaction = new TransactionScope();
        _dataProvider.BulkInsertEntities(entities);

        if (publishEvent) PublishEvents(entities.ToArray());

        transaction.Complete();
    }

    #endregion

    public async Task<TEntity?> LoadOriginalCopyAsync(TEntity entity)
    {
        return await _dataProvider.GetTable<TEntity>().FirstOrDefaultAsync(e => e.Id == entity.Id);
    }

    public async Task UpdateAsync(TEntity entity, bool publishEvent = true)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dataProvider.UpdateEntityAsync(entity);

        if (publishEvent)
        {
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public void Update(TEntity entity, bool publishEvent = true)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dataProvider.UpdateEntity(entity);

        if (publishEvent)
        {
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public async Task UpdateAsync(IList<TEntity> entities, bool publishEvent = true)
    {
        if (entities is null || !entities.Any())
        {
            return;
        }
        
        await _dataProvider.UpdateEntitiesAsync(entities);

        if (!publishEvent)
        {
            return;
        }

        foreach (var entity in entities)
        {
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public void Update(IList<TEntity> entities, bool publishEvent = true)
    {
        if (entities is null || !entities.Any())
        {
            return;
        }
        
        _dataProvider.UpdateEntities(entities);

        if (!publishEvent)
        {
            return;
        }

        foreach (var entity in entities)
        {
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public async Task DeleteAsync(TEntity entity, bool publishEvent = true)
    {
        switch (entity)
        {
            case null:
                throw new ArgumentNullException(nameof(entity));

            case ISoftDeletedEntity softDeletedEntity:
                softDeletedEntity.Deleted = true;
                await _dataProvider.UpdateEntityAsync(entity);
                break;

            default:
                await _dataProvider.DeleteEntityAsync(entity);
                break;
        }

        if (publishEvent)
        { 
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public void Delete(TEntity entity, bool publishEvent = true)
    {
        switch (entity)
        {
            case null:
                throw new ArgumentNullException(nameof(entity));
            case ISoftDeletedEntity softDeletedEntity:
                softDeletedEntity.Deleted = true;
                _dataProvider.UpdateEntity(entity);
                break;
            default:
                _dataProvider.DeleteEntity(entity);
                break;
        }

        if (publishEvent)
        {
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public async Task DeleteAsync(IList<TEntity> entities, bool publishEvent = true)
    {
        if (entities is null || !entities.Any())
        {
            return;
        }

        await DeleteAsync(entities);

        if (!publishEvent)
        {
            return;
        }

        foreach (var entity in entities)
        {
            // TODO ---> entity.RaiseDomainEvent();
        }
    }

    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var countDeletedRecords = await _dataProvider.BulkDeleteEntitiesAsync(predicate);
        transaction.Complete();

        return countDeletedRecords;
    }

    public int Delete(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var countDeletedRecords = _dataProvider.BulkDeleteEntities(predicate);
        transaction.Complete();

        return countDeletedRecords;
    }

    public async Task TruncateAsync(bool resetIdentity = false)
    {
        await _dataProvider.TruncateAsync<TEntity>(resetIdentity);
    }

    #endregion    

    #region Methods

    private async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<IStaticCacheManager, CacheKey> getCacheKey)
    {
        if (getCacheKey is null)
        {
            return await getAllAsync();
        }

        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.AllCacheKey);

        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
    }

    private IList<TEntity> GetEntities(Func<IList<TEntity>> getAll, Func<IStaticCacheManager, CacheKey> getCacheKey)
    {
        if (getCacheKey is null)
        {
            return getAll();
        }

        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.AllCacheKey);

        return _staticCacheManager.Get(cacheKey, getAll);
    }

    private async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<IStaticCacheManager, Task<CacheKey>> getCacheKey)
    {
        if (getCacheKey is null)
        {
            return await getAllAsync();
        }

        var cacheKey = await getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.AllCacheKey);

        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
    }

    private IQueryable<TEntity> AddDeletedFilter(IQueryable<TEntity> query, in bool includeDeleted)
    {
        if (includeDeleted)
        {
            return query;
        }

        if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) is null)
        {
            return query;
        }

        return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();
    }

    private async Task DeleteAsync(IList<TEntity> entities)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _dataProvider.BulkDeleteEntitiesAsync(entities);
        transaction.Complete();
    }

    private async Task DeleteAsync<T>(IList<T> entities) where T : ISoftDeletedEntity, TEntity
    {
        foreach (var entity in entities)
        {
            entity.Deleted = true;
        }

        await _dataProvider.UpdateEntitiesAsync(entities);
    }

    private async void PublishEvents(params BaseEntity[] entities)
    {
        List<IDomainEvent> domainEvents = new();

        foreach(var entity in entities)
        {
            domainEvents.AddRange(entity.GetDomainEvents());
            entity.ClearDomainEvents();
        }

        if (!domainEvents.Any()) return;

        var outboxMessages = domainEvents.Select(q => new OutboxMessage
        {
            CreatedOnUtc = DateTime.UtcNow,
            Type = q.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize<object>(q, new JsonSerializerOptions { WriteIndented = true })
        });
        await _dataProvider.BulkInsertEntitiesAsync(outboxMessages);
    }

    #endregion
}
