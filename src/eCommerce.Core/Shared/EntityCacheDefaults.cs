using eCommerce.Core.Primitives;

namespace eCommerce.Core.Shared;

public static class EntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    public static CacheKey ByIdCacheKey => new($"ecommerce.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

    public static CacheKey ByIdsCacheKey => new($"ecommerce.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

    public static CacheKey AllCacheKey => new($"ecommerce.{EntityTypeName}.all.", AllPrefix, Prefix);

    public static CacheKey ByEmailCacheKey => new($"ecommerce. {EntityTypeName} .byemail.}}", ByEmailPrefix, Prefix);

    public static string Prefix => $"ecommerce.{EntityTypeName}.";

    public static string ByIdPrefix => $"ecommerce.{EntityTypeName}.byid.";

    public static string ByIdsPrefix => $"ecommerce.{EntityTypeName}.byids.";

    public static string AllPrefix => $"ecommerce.{EntityTypeName}.all.";

    public static string ByEmailPrefix => $"ecommerce.{EntityTypeName}.byemail.";
}