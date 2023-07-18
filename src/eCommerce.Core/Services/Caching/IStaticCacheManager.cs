using eCommerce.Core.Shared;

namespace eCommerce.Core.Services.Caching;

public interface IStaticCacheManager : IDisposable
{
    Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire);

    Task<T> GetAsync<T>(CacheKey key, Func<T> acquire);

    Task<T> GetAsync<T>(CacheKey key, T defaultValue = default);

    Task<object> GetAsync(CacheKey key);

    Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters);

    Task SetAsync<T>(CacheKey key, T data);

    Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters);

    Task ClearAsync();

    T Get<T>(CacheKey key, Func<T> acquire);

    void RemoveByPrefix(string prefix, params object[] prefixParameters);

    #region Cache key

    CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters);

    CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters);

    CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters);

    #endregion
}
