using System.ComponentModel;
using YerdenYuksek.Application.Services.Public.Configuration;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Configuration;
using YerdenYuksek.Core.Domain.Configuration;
using YerdenYuksek.Core.Primitives;
using YerdenYuksek.Services.Configuration;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class SettingService : ISettingService
{
    #region Fields

    private readonly IRepository<Setting> _settingRepository;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructure and Destructure

    public SettingService(
        IRepository<Setting> settingRepository, 
        IStaticCacheManager staticCacheManager)
    {
        _settingRepository = settingRepository;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Public Methods

    public virtual async Task<IList<Setting>> GetAllSettingsAsync()
    {
        var settings = await _settingRepository.GetAllAsync(query =>
        {
            return from s in query
                   orderby s.Name
                   select s;
        }, cache => default);

        return settings;
    }

    public virtual async Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default)
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

        var setting = (settings[key]).FirstOrDefault();

        return setting != null ? CommonHelper.To<T>(setting.Value) : defaultValue;
    }

    public async Task<T> LoadSettingAsync<T>() where T : ISettings, new()
    {
        return (T)await LoadSettingAsync(typeof(T));
    }

    public async Task<ISettings> LoadSettingAsync(Type type)
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

    #region Methods

    private async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionaryAsync()
    {
        return await _staticCacheManager.GetAsync(YerdenYuksekSettingsDefaults.SettingsAllAsDictionaryCacheKey, async () =>
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
                    dictionary.Add(resourceName, new List<Setting>{ settingForCaching });
                }
                else
                {
                    dictionary[resourceName].Add(settingForCaching);
                }
            }

            return dictionary;
        });
    }

    #endregion
}
