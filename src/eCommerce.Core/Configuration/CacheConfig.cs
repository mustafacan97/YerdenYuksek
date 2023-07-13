namespace eCommerce.Core.Configuration;

public class CacheConfig : IConfig
{
    public int DefaultCacheTime { get; protected set; } = 60;

    public int ShortTermCacheTime { get; protected set; } = 3;

    public int BundledFilesCacheTime { get; protected set; } = 120;
}