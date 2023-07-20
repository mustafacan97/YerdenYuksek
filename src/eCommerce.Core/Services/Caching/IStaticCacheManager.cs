using eCommerce.Core.Shared;

namespace eCommerce.Core.Services.Caching;

public interface IStaticCacheManager : IDisposable
{
    T Get<T>(CacheKey key, Func<T> acquire);

    Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire);

    T? Get<T>(CacheKey key, T? defaultValue = default);

    Task<T?> GetAsync<T>(CacheKey key, T? defaultValue = default);

    Task<T> GetAsync<T>(CacheKey key, Func<T> acquire);    

    Task<object> GetAsync(CacheKey key);    

    Task SetAsync<T>(CacheKey key, T data);    

    Task ClearAsync();

    void Remove(CacheKey cacheKey, params object[] cacheKeyParameters);

    Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters);

    void RemoveByPrefix(string prefix, params object[] prefixParameters);

    Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters);

    #region Cache key

    CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters);

    CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters);

    CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters);

    #endregion
}
