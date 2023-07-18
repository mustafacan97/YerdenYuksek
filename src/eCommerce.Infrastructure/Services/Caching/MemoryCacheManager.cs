using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using eCommerce.Core.Configuration;
using eCommerce.Core.Shared;
using eCommerce.Core.Services.Caching;

namespace eCommerce.Infrastructure.Services.Caching;

public class MemoryCacheManager : CacheKeyService, IStaticCacheManager
{
    #region Fields

    private bool _disposed;

    private readonly IMemoryCache _memoryCache;

    private readonly ICacheKeyManager _keyManager;

    private static CancellationTokenSource _clearToken = new();

    #endregion

    #region Constructure and Destructure

    public MemoryCacheManager(
        AppSettings appSettings,
        IMemoryCache memoryCache,
        ICacheKeyManager cacheKeyManager)
        : base(appSettings)
    {
        _memoryCache = memoryCache;
        _keyManager = cacheKeyManager;
    }

    #endregion    

    #region Public Methods

    public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = PrepareKey(cacheKey, cacheKeyParameters).Key;
        _memoryCache.Remove(key);
        _keyManager.RemoveKey(key);

        return Task.CompletedTask;
    }

    public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if ((key?.CacheTime ?? 0) <= 0)
            return await acquire();

        var task = _memoryCache.GetOrCreate(
            key.Key,
            entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));
                return new Lazy<Task<T>>(acquire, true);
            });

        try
        {
            return await task!.Value;
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public async Task<T> GetAsync<T>(CacheKey key, T defaultValue = default)
    {
        var value = _memoryCache.Get<Lazy<Task<T>>>(key.Key)?.Value;

        try
        {
            return value != null ? await value : defaultValue;
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
    {
        return await GetAsync(key, () => Task.FromResult(acquire()));
    }

    public async Task<object> GetAsync(CacheKey key)
    {
        var entry = _memoryCache.Get(key.Key);
        if (entry == null)
            return null;
        try
        {
            if (entry.GetType().GetProperty("Value")?.GetValue(entry) is not Task task)
                return null;

            await task;

            return task.GetType().GetProperty("Result")!.GetValue(task);
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public Task SetAsync<T>(CacheKey key, T data)
    {
        if (data != null && (key?.CacheTime ?? 0) > 0)
            _memoryCache.Set(
                key.Key,
                new Lazy<Task<T>>(() => Task.FromResult(data), true),
                PrepareEntryOptions(key));

        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        foreach (var key in _keyManager.RemoveByPrefix(PrepareKeyPrefix(prefix, prefixParameters)))
            _memoryCache.Remove(key);

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        _clearToken.Cancel();
        _clearToken.Dispose();
        _clearToken = new CancellationTokenSource();
        _keyManager.Clear();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public T Get<T>(CacheKey key, Func<T> acquire)
    {
        return GetAsync(key, acquire).GetAwaiter().GetResult();
    }

    public void RemoveByPrefix(string prefix, params object[] prefixParameters)
    {
        RemoveByPrefixAsync(prefix, prefixParameters).Wait();
    }

    #endregion

    #region Methods

    private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
    {
        //set expiration time for the passed cache key
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
        };

        //add token to clear cache entries
        options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
        options.RegisterPostEvictionCallback(OnEviction);
        _keyManager.AddKey(key.Key);

        return options;
    }

    private void OnEviction(object key, object value, EvictionReason reason, object state)
    {
        switch (reason)
        {
            // we clean up after ourselves elsewhere
            case EvictionReason.Removed:
            case EvictionReason.Replaced:
            case EvictionReason.TokenExpired:
                break;
            // if the entry was evicted by the cache itself, we remove the key
            default:
                _keyManager.RemoveKey(key as string);
                break;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _clearToken.Dispose();

        _disposed = true;
    }

    #endregion
}
