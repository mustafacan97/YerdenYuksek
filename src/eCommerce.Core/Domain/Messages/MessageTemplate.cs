using YerdenYuksek.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Messages;

public class MessageTemplate : BaseEntity, ILocalizedEntity
{
    public string Name { get; set; }

    public string? BccEmailAddresses { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public Guid EmailAccountId { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }
}
