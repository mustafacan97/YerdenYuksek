using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Directory;

public sealed class OutboxMessage : BaseEntity
{
    public string Type { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}
