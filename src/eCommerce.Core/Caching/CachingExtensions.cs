namespace eCommerce.Core.Caching;

public static class CachingExtensions
{
    #region Public Methods

    public static T Get<T>(this IStaticCacheManager cacheManager, CacheKey key, Func<T> acquire)
    {
        return cacheManager.GetAsync(key, acquire).GetAwaiter().GetResult();
    }

    public static void RemoveByPrefix(this IStaticCacheManager cacheManager, string prefix, params object[] prefixParameters)
    {
        cacheManager.RemoveByPrefixAsync(prefix, prefixParameters).Wait();
    }

    #endregion
}
