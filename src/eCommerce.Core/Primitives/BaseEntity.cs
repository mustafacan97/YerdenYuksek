using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Core.Primitives;

public class BaseEntity
{
    #region Fields

    private readonly List<BaseEvent> _domainEvents = new();

    #endregion

    #region Constructure and Destructure

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public BaseEntity(Guid id)
    {
        Id = id;
    }

    #endregion

    #region Public Properties

    public Guid Id { get; set; }

    #endregion

    #region Public Methods

    public IReadOnlyCollection<BaseEvent> GetDomainEvent() => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion
}