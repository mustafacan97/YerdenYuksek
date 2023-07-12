using System.Linq.Expressions;
using YerdenYuksek.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace YerdenYuksek.Application.Services.Public.Localization;

public interface ILocalizedEntityService
{
    Task<IList<LocalizedProperty>> GetEntityLocalizedPropertiesAsync(Guid entityId, string localeKeyGroup, string localeKey);

    Task<string> GetLocalizedValueAsync(Guid languageId, Guid entityId, string localeKeyGroup, string localeKey);

    Task SaveLocalizedValueAsync<T>(
        T entity,
        Expression<Func<T, string>> keySelector,
        string localeValue,
        Guid languageId) where T : BaseEntity, ILocalizedEntity;

    Task SaveLocalizedValueAsync<T, TPropType>(
       T entity,
       Expression<Func<T, TPropType>> keySelector,
       TPropType localeValue,
       Guid languageId) where T : BaseEntity, ILocalizedEntity;
}
