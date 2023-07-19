using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Shared;

namespace eCommerce.Core.Services.Messages;

public static class MessageDefaults
{
    public static string NotificationListKey => "NotificationList";

    #region Caching defaults

    public static CacheKey MessageTemplatesAllCacheKey => new("ecommerce.messagetemplate.all.{0}-{1}", EntityCacheDefaults<EmailTemplate>.AllPrefix);

    public static CacheKey MessageTemplatesByNameCacheKey => new("ecommerce.messagetemplate.byname.{0}", MessageTemplatesByNamePrefix);

    public static string MessageTemplatesByNamePrefix => "ecommerce.messagetemplate.byname.{0}";

    #endregion
}