using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Messages;

public class EmailTemplate : SoftDeletedEntity, ILocalizedEntity
{
    public string Name { get; set; }

    public string? Bcc { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public Guid EmailAccountId { get; set; }

    public int? AttachedDownloadId { get; set; }

    public bool AllowDirectReply { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
