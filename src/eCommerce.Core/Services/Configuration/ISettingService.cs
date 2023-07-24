using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Interfaces;
using System.Linq.Expressions;

namespace eCommerce.Core.Services.Configuration;

public interface ISettingService
{
    void ClearCache();

    Task ClearCacheAsync();

    Task<Setting> GetSettingAsync(string key, bool loadSharedValueIfNotFound = false);

    Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default);

    string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector) where TSettings : ISettings, new();

    Task<T> LoadSettingAsync<T>() where T : ISettings, new();

    Task<ISettings?> LoadSettingAsync(Type type);

    Task SaveSettingAsync<T>(T setting) where T : ISettings, new();

    void SetSetting<T>(string key, T value, bool clearCache = true);
}