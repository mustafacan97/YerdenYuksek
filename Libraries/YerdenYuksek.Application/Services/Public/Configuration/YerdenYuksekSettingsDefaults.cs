using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Domain.Configuration;

namespace YerdenYuksek.Application.Services.Public.Configuration;

public static class YerdenYuksekSettingsDefaults
{
    public static CacheKey SettingsAllAsDictionaryCacheKey => new("YerdenYuksek.setting.all.dictionary.", YerdenYuksekEntityCacheDefaults<Setting>.Prefix); 
}