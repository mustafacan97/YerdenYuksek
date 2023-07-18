using eCommerce.Core.Caching;
using eCommerce.Core.Entities.Messages;

namespace eCommerce.Application.Services.Messages;

public static class MessageDefaults
{
    public static string NotificationListKey => "NotificationList";

    #region Caching defaults

    public static CacheKey MessageTemplatesAllCacheKey => new("YerdenYuksek.messagetemplate.all.{0}-{1}", EntityCacheDefaults<EmailTemplate>.AllPrefix);

    public static CacheKey MessageTemplatesByNameCacheKey => new("YerdenYuksek.messagetemplate.byname.{0}", MessageTemplatesByNamePrefix);

    public static string MessageTemplatesByNamePrefix => "YerdenYuksek.messagetemplate.byname.{0}";

    #endregion
}