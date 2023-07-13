using eCommerce.Core.Caching;

namespace eCommerce.Core.Caching;

public static class CustomerEntityCacheDefaults
{
    public static string Prefix => $"ecommerce.customer.";

    public static string ByEmailPrefix => $"ecommerce.customer.byemail.";

    public static CacheKey ByEmailCacheKey => new($"ecommerce.customer.byemail.{{0}}", ByEmailPrefix, Prefix);
}