using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Interfaces;
using System.Linq.Expressions;

namespace eCommerce.Core.Services.Configuration;

public interface ISettingService
{
    void ClearCache();

    Task ClearCacheAsync();

    IList<Setting> GetAllSettings();

    Task<IList<Setting>> GetAllSettingsAsync();

    Task<Setting> GetSettingAsync(string key, bool loadSharedValueIfNotFound = false);

    Setting GetSettingById(Guid settingId);

    Task<Setting> GetSettingByIdAsync(Guid settingId);

    Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default);

    string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector) where TSettings : ISettings, new();

    void InsertSetting(Setting setting, bool clearCache = true);

    Task InsertSettingAsync(Setting setting, bool clearCache = true);

    Task<T> LoadSettingAsync<T>() where T : ISettings, new();

    Task<ISettings?> LoadSettingAsync(Type type);

    Task SaveSettingAsync<T>(T setting) where T : ISettings, new();

    void SetSetting<T>(string key, T value, bool clearCache = true);

    void UpdateSetting(Setting setting, bool clearCache = true);

    Task UpdateSettingAsync(Setting setting, bool clearCache = true);
}