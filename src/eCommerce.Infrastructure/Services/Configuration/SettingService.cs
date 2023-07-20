using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Shared;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Services.Configuration;

namespace eCommerce.Infrastructure.Services.Configuration;

public class SettingService : ISettingService
{
    #region Fields

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public SettingService(
        IStaticCacheManager staticCacheManager,
        IUnitOfWork unitOfWork)
    {
        _staticCacheManager = staticCacheManager;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    #region Commands

    public void ClearCache()
    {
        _staticCacheManager.RemoveByPrefix(EntityCacheDefaults<Setting>.Prefix);
    }

    public async Task ClearCacheAsync()
    {
        await _staticCacheManager.RemoveByPrefixAsync(EntityCacheDefaults<Setting>.Prefix);
    }

    public void InsertSetting(Setting setting, bool clearCache = true)
    {
        _unitOfWork.GetRepository<Setting>().Insert(setting);

        if (clearCache)
        {
            ClearCache();
        }

        _unitOfWork.SaveChanges();
    }

    public async Task InsertSettingAsync(Setting setting, bool clearCache = true)
    {
        await _unitOfWork.GetRepository<Setting>().InsertAsync(setting);

        if (clearCache)
        {
            await ClearCacheAsync();
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SaveSettingAsync<T>(T settings) where T : ISettings, new()
    {
        foreach (var prop in typeof(T).GetProperties())
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
            {
                continue;
            }

            var key = typeof(T).Name + "." + prop.Name;
            var value = prop.GetValue(settings, null);
            if (value != null)
            {
                await SetSettingAsync(prop.PropertyType, key, value, false);
            }
            else
            {
                await SetSettingAsync(key, string.Empty, false);
            }
        }

        await ClearCacheAsync();
        await _unitOfWork.SaveChangesAsync();
    }

    public void SetSetting<T>(string key, T value, bool clearCache = true)
    {
        SetSetting(typeof(T), key, value, clearCache);
    }

    public async Task SetSettingAsync<T>(string key, T value, bool clearCache = true)
    {
        await SetSettingAsync(typeof(T), key, value, clearCache);
    }

    public void UpdateSetting(Setting setting, bool clearCache = true)
    {
        if (setting is null)
        {
            throw new ArgumentNullException(nameof(setting));
        }

        _unitOfWork.GetRepository<Setting>().Update(setting);

        if (clearCache)
        {
            ClearCache();
        }

        _unitOfWork.SaveChanges();
    }

    public async Task UpdateSettingAsync(Setting setting, bool clearCache = true)
    {
        if (setting is null)
        {
            throw new ArgumentNullException(nameof(setting));
        }

        _unitOfWork.GetRepository<Setting>().Update(setting);

        if (clearCache)
        {
            await ClearCacheAsync();
        }

        await _unitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Queries

    public IList<Setting> GetAllSettings()
    {
        var settings = _unitOfWork.GetRepository<Setting>().GetAll(query =>
        {
            return from s in query
                   orderby s.Name
                   select s;
        }, cache => default);

        return settings;
    }

    public async Task<IList<Setting>> GetAllSettingsAsync()
    {
        var settings = await _unitOfWork.GetRepository<Setting>().GetAllAsync(query =>
        {
            return from s in query
                   orderby s.Name
                   select s;
        }, cache => default);

        return settings;
    }

    public Setting GetSettingById(Guid settingId)
    {
        return _unitOfWork.GetRepository<Setting>().GetById(settingId);
    }

    public async Task<Setting> GetSettingByIdAsync(Guid settingId)
    {
        return await _unitOfWork.GetRepository<Setting>().GetByIdAsync(settingId);
    }

    public async Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default)
    {
        if (string.IsNullOrEmpty(key))
        {
            return defaultValue;
        }

        var settings = await GetAllSettingsDictionaryAsync();
        key = key.Trim().ToLowerInvariant();

        if (!settings.ContainsKey(key))
        {
            return defaultValue;
        }

        var setting = settings[key].FirstOrDefault();

        return !string.IsNullOrEmpty(setting?.Value) ? CommonHelper.To<T>(setting.Value) : defaultValue;
    }

    public async Task<Setting> GetSettingAsync(string key, bool loadSharedValueIfNotFound = false)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        var settings = await GetAllSettingsDictionaryAsync();
        key = key.Trim().ToLowerInvariant();
        if (!settings.ContainsKey(key))
        {
            return null;
        }

        var settingsByKey = settings[key];
        var setting = settingsByKey.FirstOrDefault();

        return setting != null ? await GetSettingByIdAsync(setting.Id) : null;
    }

    public string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector) where TSettings : ISettings, new()
    {
        if (keySelector.Body is not MemberExpression member)
        {
            throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
        }

        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
        }

        var key = $"{typeof(TSettings).Name}.{propInfo.Name}";

        return key;
    }

    public async Task<T> LoadSettingAsync<T>() where T : ISettings, new()
    {
        return (T)await LoadSettingAsync(typeof(T));
    }

    public async Task<ISettings?> LoadSettingAsync(Type type)
    {
        var settings = Activator.CreateInstance(type);

        foreach (var prop in type.GetProperties())
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            var key = type.Name + "." + prop.Name;
            var setting = await GetSettingByKeyAsync<string>(key);

            if (setting == null)
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
            {
                continue;
            }

            if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
            {
                continue;
            }

            var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);
            prop.SetValue(settings, value, null);
        }

        return settings as ISettings;
    }

    #endregion

    #endregion

    #region Methods

    private IDictionary<string, IList<Setting>> GetAllSettingsDictionary()
    {
        return _staticCacheManager.Get(SettingsDefaults.SettingsAllAsDictionaryCacheKey, () =>
        {
            var settings = GetAllSettings();
            var dictionary = new Dictionary<string, IList<Setting>>();
            foreach (var s in settings)
            {
                var resourceName = s.Name.ToLowerInvariant();
                var settingForCaching = new Setting
                {
                    Id = s.Id,
                    Name = s.Name,
                    Value = s.Value
                };
                if (!dictionary.ContainsKey(resourceName))
                {
                    dictionary.Add(resourceName, new List<Setting> { settingForCaching });
                }
                else
                {
                    dictionary[resourceName].Add(settingForCaching);
                }
            }

            return dictionary;
        });
    }

    private async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionaryAsync()
    {
        return await _staticCacheManager.GetAsync(SettingsDefaults.SettingsAllAsDictionaryCacheKey, async () =>
        {
            var settings = await GetAllSettingsAsync();

            var dictionary = new Dictionary<string, IList<Setting>>();
            foreach (var s in settings)
            {
                var resourceName = s.Name.ToLowerInvariant();
                var settingForCaching = new Setting
                {
                    Id = s.Id,
                    Name = s.Name,
                    Value = s.Value
                };

                if (!dictionary.ContainsKey(resourceName))
                {
                    dictionary.Add(resourceName, new List<Setting> { settingForCaching });
                }
                else
                {
                    dictionary[resourceName].Add(settingForCaching);
                }
            }

            return dictionary;
        });
    }

    private void SetSetting(Type type, string key, object value, bool clearCache = true)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        key = key.Trim().ToLowerInvariant();

        var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);
        var allSettings = GetAllSettingsDictionary();
        var settingForCaching = allSettings.TryGetValue(key, out var settings) ?
            settings.FirstOrDefault() : null;
        if (settingForCaching != null)
        {
            var setting = GetSettingById(settingForCaching.Id);
            setting.Value = valueStr;
            UpdateSetting(setting, clearCache);
        }
        else
        {
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Name = key,
                Value = valueStr
            };
            InsertSetting(setting, clearCache);
        }
    }

    private async Task SetSettingAsync(Type type, string key, object value, bool clearCache = true)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        key = key.Trim().ToLowerInvariant();

        var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);
        var allSettings = await GetAllSettingsDictionaryAsync();
        var settingForCaching = allSettings.TryGetValue(key, out var settings) ?
            settings.FirstOrDefault() : null;
        if (settingForCaching != null)
        {
            var setting = await GetSettingByIdAsync(settingForCaching.Id);
            setting.Value = valueStr;
            await UpdateSettingAsync(setting, clearCache);
        }
        else
        {
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Name = key,
                Value = valueStr
            };

            await InsertSettingAsync(setting, clearCache);
        }
    }

    #endregion
}
