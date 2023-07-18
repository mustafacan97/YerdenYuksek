using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Shared;

namespace eCommerce.Application.Services.Configuration;

public static class SettingsDefaults
{
    public static CacheKey SettingsAllAsDictionaryCacheKey => new("ecommerce.setting.all.dictionary.", EntityCacheDefaults<Setting>.Prefix);
}