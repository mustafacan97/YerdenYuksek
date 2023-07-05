using YerdenYuksek.Core.Configuration;

namespace YerdenYuksek.Core.Domain.Messages;

public class MessageSettings : ISettings
{
    public bool UsePopupNotifications { get; set; }

    public bool UseDefaultEmailAccountForSendStoreOwnerEmails { get; set; }
}
