using System.Linq.Expressions;
using eCommerce.Core.Configuration;
using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Application.Services.Localization;

public interface ILocalizationService
{
    Task DeleteLocaleStringResourceAsync(LocaleStringResource localeStringResource);

    Task<LocaleStringResource> GetLocaleStringResourceByIdAsync(Guid localeStringResourceId);

    Task<LocaleStringResource?> GetLocaleStringResourceByNameAsync(string resourceName, Guid languageId, bool logIfNotFound = true);

    LocaleStringResource? GetLocaleStringResourceByName(string resourceName, Guid languageId, bool logIfNotFound = true);

    Task InsertLocaleStringResourceAsync(LocaleStringResource localeStringResource);

    Task UpdateLocaleStringResourceAsync(LocaleStringResource localeStringResource);

    void UpdateLocaleStringResource(LocaleStringResource localeStringResource);

    Task<Dictionary<string, KeyValuePair<Guid, string>>> GetAllResourceValuesAsync(Guid languageId, bool? loadPublicLocales);

    Task<string> GetResourceAsync(string resourceKey);

    Task<string> GetResourceAsync(
        string resourceKey,
        Guid languageId,
        bool logIfNotFound = true,
        string defaultValue = "",
        bool returnEmptyIfNotFound = false);

    Task<TPropType> GetLocalizedAsync<TEntity, TPropType>(
        TEntity entity,
        Expression<Func<TEntity, TPropType>> keySelector,
        Guid? languageId = null,
        bool returnDefaultValue = true,
        bool ensureTwoPublishedLanguages = true)
        where TEntity : BaseEntity, ILocalizedEntity;

    Task<string> GetLocalizedSettingAsync<TSettings>(
        TSettings settings,
        Expression<Func<TSettings, string>> keySelector,
        Guid languageId,
        bool returnDefaultValue = true,
        bool ensureTwoPublishedLanguages = true)
        where TSettings : ISettings, new();

    Task SaveLocalizedSettingAsync<TSettings>(
        TSettings settings,
        Expression<Func<TSettings, string>> keySelector,
        Guid languageId,
        string value) where TSettings : ISettings, new();

    Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue, Guid? languageId = null) where TEnum : struct;

    Task AddOrUpdateLocaleResourceAsync(string resourceName, string resourceValue, string? languageCulture = null);

    Task AddOrUpdateLocaleResourceAsync(IDictionary<string, string> resources, Guid? languageId = null);

    void AddOrUpdateLocaleResource(IDictionary<string, string> resources, Guid? languageId = null);

    Task DeleteLocaleResourceAsync(string resourceName);

    Task DeleteLocaleResourcesAsync(IList<string> resourceNames, Guid? languageId = null);

    void DeleteLocaleResources(IList<string> resourceNames, Guid? languageId = null);

    Task DeleteLocaleResourcesAsync(string resourceNamePrefix, Guid? languageId = null);
}