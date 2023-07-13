using YerdenYuksek.Core.Caching;

namespace eCommerce.Application.Services.Configuration;

public static class SettingsDefaults
{
    public static CacheKey SettingsAllAsDictionaryCacheKey => new("ecommerce.setting.all.dictionary.", YerdenYuksekEntityCacheDefaults<Setting>.Prefix);
}