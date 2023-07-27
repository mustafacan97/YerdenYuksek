using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Shared;

namespace eCommerce.Core.Services.Localization;

public static class LocalizationDefaults
{
    #region Locales

    public static string AdminLocaleStringResourcesPrefix => "Admin.";

    public static string EnumLocaleStringResourcesPrefix => "Enums.";

    public static string PermissionLocaleStringResourcesPrefix => "Permission.";

    public static string PluginNameLocaleStringResourcesPrefix => "Plugins.FriendlyName.";

    #endregion

    #region Caching defaults

    #region Languages

    public static CacheKey LanguagesAllCacheKey => new("ecommerce.language.all.{0}-{1}", LanguagesByStorePrefix, EntityCacheDefaults<Language>.AllPrefix);

    public static string LanguagesByStorePrefix => "ecommerce.language.all.{0}";

    #endregion

    #region Locales

    public static CacheKey LocaleStringResourcesAllPublicCacheKey => new("ecommerce.localestringresource.bylanguage.public.{0}", EntityCacheDefaults<LocaleStringResource>.Prefix);

    public static CacheKey LocaleStringResourcesAllAdminCacheKey => new("ecommerce.localestringresource.bylanguage.admin.{0}", EntityCacheDefaults<LocaleStringResource>.Prefix);

    public static CacheKey LocaleStringResourcesAllCacheKey => new("ecommerce.localestringresource.bylanguage.{0}", EntityCacheDefaults<LocaleStringResource>.Prefix);

    public static CacheKey LocaleStringResourcesByNameCacheKey => new("ecommerce.localestringresource.byname.{0}-{1}", LocaleStringResourcesByNamePrefix, EntityCacheDefaults<LocaleStringResource>.Prefix);

    public static string LocaleStringResourcesByNamePrefix => "ecommerce.localestringresource.byname.{0}";

    #endregion

    #region Localized properties

    public static CacheKey LocalizedPropertyCacheKey => new("ecommerce.localizedproperty.value.{0}-{1}-{2}-{3}");

    public static CacheKey LocalizedPropertiesCacheKey => new("ecommerce.localizedproperty.all.{0}-{1}-{2}");

    public static CacheKey LocalizedPropertyLookupCacheKey => new("ecommerce.localizedproperty.value.{0}");

    #endregion

    #endregion
}
