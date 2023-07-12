using eCommerce.Core.Primitives;

namespace YerdenYuksek.Core.Caching;

public static class YerdenYuksekEntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    public static CacheKey ByIdCacheKey => new($"Nop.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

    public static CacheKey ByIdsCacheKey => new($"Nop.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

    public static CacheKey AllCacheKey => new($"Nop.{EntityTypeName}.all.", AllPrefix, Prefix);

    public static string Prefix => $"Nop.{EntityTypeName}.";

    public static string ByIdPrefix => $"Nop.{EntityTypeName}.byid.";

    public static string ByIdsPrefix => $"Nop.{EntityTypeName}.byids.";

    public static string AllPrefix => $"Nop.{EntityTypeName}.all.";
}