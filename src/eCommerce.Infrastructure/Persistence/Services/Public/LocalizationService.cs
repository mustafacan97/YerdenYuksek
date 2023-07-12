using eCommerce.Core.Helpers;
using eCommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;
using YerdenYuksek.Application.Services.Public.Configuration;
using YerdenYuksek.Application.Services.Public.Localization;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Configuration;
using YerdenYuksek.Core.Domain.Localization;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class LocalizationService : ILocalizationService
{
    #region Fields

    private readonly LocalizationSettings _localizationSettings;

    private readonly IStaticCacheManager _staticCacheManager;

    private readonly ILanguageService _languageService;

    private readonly ILocalizedEntityService _localizedEntityService;

    private readonly ISettingService _settingService;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IWorkContext _workContext;

    #endregion

    #region Constructure and Destructure

    public LocalizationService(
        ILanguageService languageService,
        IUnitOfWork unitOfWork,
        IStaticCacheManager staticCacheManager,
        IWorkContext workContext,
        ILocalizedEntityService localizedEntityService,
        ISettingService settingService,
        LocalizationSettings localizationSettings)
    {
        _languageService = languageService;
        _unitOfWork = unitOfWork;
        _staticCacheManager = staticCacheManager;
        _workContext = workContext;
        _localizedEntityService = localizedEntityService;
        _settingService = settingService;
        _localizationSettings = localizationSettings;
    }

    #endregion

    #region Public Methods

    public void AddOrUpdateLocaleResource(IDictionary<string, string> resources, Guid? languageId = null)
    {
        var resourcesToInsert = UpdateLocaleResource(resources, languageId, false);

        if (resourcesToInsert.Any())
        {
            var locales = _languageService.GetAllLanguages(true)
                .Where(language => !languageId.HasValue || language.Id == languageId.Value)
                .SelectMany(language => resourcesToInsert.Select(resource => new LocaleStringResource
                {
                    LanguageId = language.Id,
                    ResourceName = resource.Key.Trim().ToLowerInvariant(),
                    ResourceValue = resource.Value
                }))
                .ToList();

            _unitOfWork.GetRepository<LocaleStringResource>().Insert(locales);
            _unitOfWork.SaveChanges();
        }
        
        _staticCacheManager.RemoveByPrefix(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
    }

    public async Task AddOrUpdateLocaleResourceAsync(string resourceName, string resourceValue, string? languageCulture = null)
    {
        foreach (var lang in await _languageService.GetAllLanguagesAsync(true))
        {
            if (!string.IsNullOrEmpty(languageCulture) && !languageCulture.Equals(lang.LanguageCulture))
            {
                continue;
            }

            var lsr = await GetLocaleStringResourceByNameAsync(resourceName, lang.Id, false);
            if (lsr is null)
            {
                lsr = new LocaleStringResource
                {
                    LanguageId = lang.Id,
                    ResourceName = resourceName,
                    ResourceValue = resourceValue
                };
                await InsertLocaleStringResourceAsync(lsr);
            }
            else
            {
                lsr.ResourceValue = resourceValue;
                await UpdateLocaleStringResourceAsync(lsr);
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddOrUpdateLocaleResourceAsync(IDictionary<string, string> resources, Guid? languageId = null)
    {
        var resourcesToInsert = await UpdateLocaleResourceAsync(resources, languageId, false);

        if (resourcesToInsert.Any())
        {
            var locales = (await _languageService.GetAllLanguagesAsync(true))
                .Where(language => !languageId.HasValue || language.Id == languageId.Value)
                .SelectMany(language => resourcesToInsert.Select(resource => new LocaleStringResource
                {
                    LanguageId = language.Id,
                    ResourceName = resource.Key.Trim().ToLowerInvariant(),
                    ResourceValue = resource.Value
                }))
                .ToList();

            await _unitOfWork.GetRepository<LocaleStringResource>().InsertAsync(locales);
        }

        await _staticCacheManager.RemoveByPrefixAsync(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteLocaleResourceAsync(string resourceName)
    {
        foreach (var lang in await _languageService.GetAllLanguagesAsync(true))
        {
            var lsr = await GetLocaleStringResourceByNameAsync(resourceName, lang.Id, false);
            if (lsr is not null)
            {
                await DeleteLocaleStringResourceAsync(lsr);
            }
        }

        _unitOfWork.SaveChanges();
    }

    public void DeleteLocaleResources(IList<string> resourceNames, Guid? languageId = null)
    {
        var resources = _unitOfWork.GetRepository<LocaleStringResource>().GetAll(query => 
        {
            query = query.Where(q =>
                (!languageId.HasValue || q.LanguageId == languageId) &&
                resourceNames.Contains(q.ResourceName, StringComparer.InvariantCultureIgnoreCase));

            return query;
        });

        if (resources is null || !resources.Any())
        {
            return;
        }

        foreach(var resource in resources)
        {
            _unitOfWork.GetRepository<LocaleStringResource>().Delete(resource);
        }

        _staticCacheManager.RemoveByPrefix(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
        _unitOfWork.SaveChanges();
    }

    public async Task DeleteLocaleResourcesAsync(IList<string> resourceNames, Guid? languageId = null)
    {
        var resources = await _unitOfWork.GetRepository<LocaleStringResource>().GetAllAsync(query =>
        {
            query = query.Where(q =>
                (!languageId.HasValue || q.LanguageId == languageId) &&
                resourceNames.Contains(q.ResourceName, StringComparer.InvariantCultureIgnoreCase));

            return query;
        });

        if (resources is null || !resources.Any())
        {
            return;
        }

        foreach (var resource in resources)
        {
            _unitOfWork.GetRepository<LocaleStringResource>().Delete(resource);
        }

        await _staticCacheManager.RemoveByPrefixAsync(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteLocaleResourcesAsync(string resourceNamePrefix, Guid? languageId = null)
    {
        var resources = await _unitOfWork.GetRepository<LocaleStringResource>().GetAllAsync(query =>
        {
            query = query.Where(q =>
                (!languageId.HasValue || q.LanguageId == languageId) &&
                !string.IsNullOrEmpty(q.ResourceName) &&
                q.ResourceName.StartsWith(resourceNamePrefix, StringComparison.InvariantCultureIgnoreCase));

            return query;
        });

        if (resources is null || !resources.Any())
        {
            return;
        }

        foreach (var resource in resources)
        {
            _unitOfWork.GetRepository<LocaleStringResource>().Delete(resource);
        }

        await _staticCacheManager.RemoveByPrefixAsync(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        _unitOfWork.GetRepository<LocaleStringResource>().Delete(localeStringResource);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Dictionary<string, KeyValuePair<Guid, string>>> GetAllResourceValuesAsync(Guid languageId, bool? loadPublicLocales)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(YerdenYuksekLocalizationDefaults.LocaleStringResourcesAllCacheKey, languageId);
        var allLocales = await _staticCacheManager.GetAsync<Dictionary<string, KeyValuePair<Guid, string>>>(key);

        if (!loadPublicLocales.HasValue || allLocales is not null)
        {
            var rez = allLocales ?? await _staticCacheManager.GetAsync(key, () =>
            {
                var resources = _unitOfWork.GetRepository<LocaleStringResource>().GetAll(query =>
                {
                    query = query
                        .Where(q => q.LanguageId == languageId)
                        .OrderBy(q => q.ResourceName);

                    return query;
                });

                return ResourceValuesToDictionary(resources);
            });

            await _staticCacheManager.RemoveAsync(YerdenYuksekLocalizationDefaults.LocaleStringResourcesAllPublicCacheKey, languageId);
            await _staticCacheManager.RemoveAsync(YerdenYuksekLocalizationDefaults.LocaleStringResourcesAllAdminCacheKey, languageId);

            return rez;
        }

        key = _staticCacheManager.PrepareKeyForDefaultCache(loadPublicLocales.Value
            ? YerdenYuksekLocalizationDefaults.LocaleStringResourcesAllPublicCacheKey
            : YerdenYuksekLocalizationDefaults.LocaleStringResourcesAllAdminCacheKey,
            languageId);

        return await _staticCacheManager.GetAsync(key, async () =>
        {
            var resources = await _unitOfWork.GetRepository<LocaleStringResource>().GetAllAsync(query =>
            {
                query = query.Where(q => q.LanguageId == languageId);

                if (loadPublicLocales.Value)
                {
                    query = query.Where(r => !r.ResourceName.StartsWith(YerdenYuksekLocalizationDefaults.AdminLocaleStringResourcesPrefix));
                }
                else
                {
                    query = query.Where(r => r.ResourceName.StartsWith(YerdenYuksekLocalizationDefaults.AdminLocaleStringResourcesPrefix));
                }
                
                query = query.OrderBy(q => q.ResourceName);

                return query;
            });

            return ResourceValuesToDictionary(resources);
        });
    }

    public async Task<LocaleStringResource> GetLocaleStringResourceByIdAsync(Guid localeStringResourceId)
    {
        return await _unitOfWork.GetRepository<LocaleStringResource>()
            .GetByIdAsync(localeStringResourceId, cache => default);
    }

    public LocaleStringResource? GetLocaleStringResourceByName(string resourceName, Guid languageId, bool logIfNotFound = true)
    {
        var query = from lsr in _unitOfWork.GetRepository<LocaleStringResource>().Table
                    orderby lsr.ResourceName
                    where lsr.LanguageId == languageId && lsr.ResourceName == resourceName.ToLowerInvariant()
                    select lsr;

        var localeStringResource = query.FirstOrDefault();

        if (localeStringResource is null && logIfNotFound)
        {
            return null;
        }

        return localeStringResource;
    }

    public async Task<LocaleStringResource?> GetLocaleStringResourceByNameAsync(string resourceName, Guid languageId, bool logIfNotFound = true)
    {
        var query = from lsr in _unitOfWork.GetRepository<LocaleStringResource>().Table
                    orderby lsr.ResourceName
                    where lsr.LanguageId == languageId && lsr.ResourceName == resourceName.ToLowerInvariant()
                    select lsr;

        var localeStringResource = await query.FirstOrDefaultAsync();

        if (localeStringResource == null && logIfNotFound)
        {
            return null;
        }

        return localeStringResource;
    }

    public async Task<TPropType> GetLocalizedAsync<TEntity, TPropType>(
        TEntity entity,
        Expression<Func<TEntity, TPropType>> keySelector,
        Guid? languageId = null,
        bool returnDefaultValue = true,
        bool ensureTwoPublishedLanguages = true) where TEntity : BaseEntity, ILocalizedEntity
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (keySelector.Body is not MemberExpression member)
        {
            throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
        }

        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
        }

        var result = default(TPropType);
        var resultStr = string.Empty;

        var localeKeyGroup = entity.GetType().Name;
        var localeKey = propInfo.Name;

        if (!languageId.HasValue)
        {
            var workingLanguage = await _workContext.GetWorkingLanguageAsync();
            languageId = workingLanguage.Id;
        }

        var loadLocalizedValue = true;
        if (ensureTwoPublishedLanguages)
        {
            var totalPublishedLanguages = (await _unitOfWork.GetRepository<Language>().GetAllAsync()).Count;
            loadLocalizedValue = totalPublishedLanguages >= 2;
        }
        
        if (loadLocalizedValue)
        {
            resultStr = await _localizedEntityService.GetLocalizedValueAsync(languageId.Value, entity.Id, localeKeyGroup, localeKey);
            if (!string.IsNullOrEmpty(resultStr))
            {
                result = CommonHelper.To<TPropType>(resultStr);
            }
        }
        
        if (!string.IsNullOrEmpty(resultStr) || !returnDefaultValue)
        {
            return result;
        }

        var localizer = keySelector.Compile();
        result = localizer(entity);

        return result;
    }

    public async Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue, Guid? languageId = null) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        var workingLanguage = await _workContext.GetWorkingLanguageAsync();
        var resourceName = $"{YerdenYuksekLocalizationDefaults.EnumLocaleStringResourcesPrefix}{typeof(TEnum)}.{enumValue}";
        var result = await GetResourceAsync(resourceName, languageId ?? workingLanguage.Id, false, string.Empty, true);

        if (string.IsNullOrEmpty(result))
        {
            result = CommonHelper.ConvertEnum(enumValue.ToString());
        }

        return result;
    }

    public virtual async Task<string> GetLocalizedSettingAsync<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector,
            Guid languageId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where TSettings : ISettings, new()
    {
        var key = _settingService.GetSettingKey(settings, keySelector);        
        var setting = await _settingService.GetSettingAsync(key, loadSharedValueIfNotFound: true);

        if (setting == null)
        {
            return null;
        }

        return await GetLocalizedAsync(setting, x => x.Value, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
    }

    public async Task<string> GetResourceAsync(string resourceKey)
    {
        var workingLanguage = await _workContext.GetWorkingLanguageAsync();

        if (workingLanguage is not null)
        {
            return await GetResourceAsync(resourceKey, workingLanguage.Id);
        }

        return string.Empty;
    }

    public async Task<string> GetResourceAsync(
        string resourceKey,
        Guid languageId,
        bool logIfNotFound = true,
        string defaultValue = "",
        bool returnEmptyIfNotFound = false)
    {
        var result = string.Empty;

        if (resourceKey == null)
        {
            resourceKey = string.Empty;
        }

        resourceKey = resourceKey.Trim().ToLowerInvariant();
        if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
        {
            var resources = await GetAllResourceValuesAsync(languageId, !resourceKey.StartsWith(YerdenYuksekLocalizationDefaults.AdminLocaleStringResourcesPrefix, StringComparison.InvariantCultureIgnoreCase));
            if (resources.ContainsKey(resourceKey))
            {
                result = resources[resourceKey].Value;
            }
        }
        else
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(
                YerdenYuksekLocalizationDefaults.LocaleStringResourcesByNameCacheKey,
                languageId,
                resourceKey);

            var query = from l in _unitOfWork.GetRepository<LocaleStringResource>().Table
                        where l.ResourceName == resourceKey
                              && l.LanguageId == languageId
                        select l.ResourceValue;

            var lsr = await _staticCacheManager.GetAsync(key, async () => await query.FirstOrDefaultAsync());

            if (lsr != null)
                result = lsr;
        }

        if (!string.IsNullOrEmpty(result))
        {
            return result;
        }

        if (logIfNotFound)
        {
            // TODO
        }

        if (!string.IsNullOrEmpty(defaultValue))
        {
            result = defaultValue;
        }
        else
        {
            if (!returnEmptyIfNotFound)
                result = resourceKey;
        }

        return result;
    }

    public async Task InsertLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        if (!string.IsNullOrEmpty(localeStringResource?.ResourceName))
        {
            localeStringResource.ResourceName = localeStringResource.ResourceName.Trim().ToLowerInvariant();
        }

        await _unitOfWork.GetRepository<LocaleStringResource>().InsertAsync(localeStringResource);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task SaveLocalizedSettingAsync<TSettings>(
        TSettings settings,
        Expression<Func<TSettings, string>> keySelector,
        Guid languageId,
        string value) where TSettings : ISettings, new()
    {
        var key = _settingService.GetSettingKey(settings, keySelector);
        var setting = await _settingService.GetSettingAsync(key);

        if (setting == null)
        {
            return;
        }

        await _localizedEntityService.SaveLocalizedValueAsync(setting, x => x.Value, value, languageId);
    }

    public void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
    {
        _unitOfWork.GetRepository<LocaleStringResource>().Update(localeStringResource);
    }

    public async Task UpdateLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        _unitOfWork.GetRepository<LocaleStringResource>().Update(localeStringResource);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Methods

    private IDictionary<string, string> UpdateLocaleResource(IDictionary<string, string> resources, Guid? languageId = null, bool clearCache = true)
    {
        var localResources = new Dictionary<string, string>(resources, StringComparer.InvariantCultureIgnoreCase);
        var keys = localResources.Keys.Select(key => key.ToLowerInvariant()).ToArray();
        var resourcesToUpdate = _unitOfWork.GetRepository<LocaleStringResource>().GetAll(query =>
        {
            var rez = query.Where(p => !languageId.HasValue || p.LanguageId == languageId)
                .Where(p => keys.Contains(p.ResourceName.ToLower()));

            return rez;
        });

        var existsResources = new List<string>();

        foreach (var localeStringResource in resourcesToUpdate.ToList())
        {
            var newValue = localResources[localeStringResource.ResourceName];

            if (localeStringResource.ResourceValue.Equals(newValue))
                resourcesToUpdate.Remove(localeStringResource);

            localeStringResource.ResourceValue = newValue;
            existsResources.Add(localeStringResource.ResourceName);
        }

        _unitOfWork.GetRepository<LocaleStringResource>().Update(resourcesToUpdate);

        if (clearCache)
        {
            _staticCacheManager.RemoveByPrefix(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        return localResources
            .Where(item => !existsResources.Contains(item.Key, StringComparer.InvariantCultureIgnoreCase))
            .ToDictionary(p => p.Key, p => p.Value);
    }

    private async Task<IDictionary<string, string>> UpdateLocaleResourceAsync(IDictionary<string, string> resources, Guid? languageId = null, bool clearCache = true)
    {
        var localResources = new Dictionary<string, string>(resources, StringComparer.InvariantCultureIgnoreCase);
        var keys = localResources.Keys.Select(key => key.ToLowerInvariant()).ToArray();
        var resourcesToUpdate = await _unitOfWork.GetRepository<LocaleStringResource>().GetAllAsync(query =>
        {
            var rez = query
                .Where(p => !languageId.HasValue || p.LanguageId == languageId)
                .Where(p => keys.Contains(p.ResourceName.ToLower()));

            return rez;
        });

        var existsResources = new List<string>();

        foreach (var localeStringResource in resourcesToUpdate.ToList())
        {
            var newValue = localResources[localeStringResource.ResourceName];

            if (localeStringResource.ResourceValue.Equals(newValue))
            {
                resourcesToUpdate.Remove(localeStringResource);
            }

            localeStringResource.ResourceValue = newValue;
            existsResources.Add(localeStringResource.ResourceName);
        }

        _unitOfWork.GetRepository<LocaleStringResource>().Update(resourcesToUpdate);

        if (clearCache)
        {
            await _staticCacheManager.RemoveByPrefixAsync(YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        return localResources
            .Where(item => !existsResources.Contains(item.Key, StringComparer.InvariantCultureIgnoreCase))
            .ToDictionary(p => p.Key, p => p.Value);
    }

    private static Dictionary<string, KeyValuePair<Guid, string>> ResourceValuesToDictionary(IEnumerable<LocaleStringResource> locales)
    {
        var dictionary = new Dictionary<string, KeyValuePair<Guid, string>>();
        foreach (var locale in locales)
        {
            var resourceName = locale.ResourceName.ToLowerInvariant();
            if (!dictionary.ContainsKey(resourceName))
            {
                dictionary.Add(resourceName, new KeyValuePair<Guid, string>(locale.Id, locale.ResourceValue));
            }
        }

        return dictionary;
    }    

    #endregion
}
