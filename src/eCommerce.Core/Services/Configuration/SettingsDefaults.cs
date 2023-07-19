using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Shared;

namespace eCommerce.Core.Services.Configuration;

public static class SettingsDefaults
{
    public static CacheKey SettingsAllAsDictionaryCacheKey => new("ecommerce.setting.all.dictionary.", EntityCacheDefaults<Setting>.Prefix);
}