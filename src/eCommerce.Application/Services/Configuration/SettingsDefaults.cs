using eCommerce.Core.Caching;
using eCommerce.Core.Domain.Configuration;

namespace eCommerce.Application.Services.Configuration;

public static class SettingsDefaults
{
    public static CacheKey SettingsAllAsDictionaryCacheKey => new("ecommerce.setting.all.dictionary.", EntityCacheDefaults<Setting>.Prefix);
}