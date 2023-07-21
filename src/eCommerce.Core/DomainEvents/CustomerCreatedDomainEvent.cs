using eCommerce.Core.Primitives;

namespace eCommerce.Core.DomainEvents;

public sealed record CustomerCreatedDomainEvent(Guid CustomerId) : IDomainEvent
{
}
