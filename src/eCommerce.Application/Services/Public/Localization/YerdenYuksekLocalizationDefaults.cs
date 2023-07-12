﻿using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Domain.Localization;

namespace YerdenYuksek.Application.Services.Public.Localization;

public static class YerdenYuksekLocalizationDefaults
{
    #region Locales

    public static string AdminLocaleStringResourcesPrefix => "Admin.";

    public static string EnumLocaleStringResourcesPrefix => "Enums.";

    public static string PermissionLocaleStringResourcesPrefix => "Permission.";

    public static string PluginNameLocaleStringResourcesPrefix => "Plugins.FriendlyName.";

    #endregion

    #region Caching defaults

    #region Languages

    public static CacheKey LanguagesAllCacheKey => new("YerdenYuksek.language.all.{0}-{1}", LanguagesByStorePrefix, YerdenYuksekEntityCacheDefaults<Language>.AllPrefix);

    public static string LanguagesByStorePrefix => "YerdenYuksek.language.all.{0}";

    #endregion

    #region Locales

    public static CacheKey LocaleStringResourcesAllPublicCacheKey => new("YerdenYuksek.localestringresource.bylanguage.public.{0}", YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);

    public static CacheKey LocaleStringResourcesAllAdminCacheKey => new("YerdenYuksek.localestringresource.bylanguage.admin.{0}", YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);

    public static CacheKey LocaleStringResourcesAllCacheKey => new("YerdenYuksek.localestringresource.bylanguage.{0}", YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);

    public static CacheKey LocaleStringResourcesByNameCacheKey => new("YerdenYuksek.localestringresource.byname.{0}-{1}", LocaleStringResourcesByNamePrefix, YerdenYuksekEntityCacheDefaults<LocaleStringResource>.Prefix);

    public static string LocaleStringResourcesByNamePrefix => "YerdenYuksek.localestringresource.byname.{0}";

    #endregion

    #region Localized properties

    public static CacheKey LocalizedPropertyCacheKey => new("YerdenYuksek.localizedproperty.value.{0}-{1}-{2}-{3}");

    public static CacheKey LocalizedPropertiesCacheKey => new("YerdenYuksek.localizedproperty.all.{0}-{1}-{2}");

    public static CacheKey LocalizedPropertyLookupCacheKey => new("YerdenYuksek.localizedproperty.value.{0}");

    #endregion

    #endregion
}