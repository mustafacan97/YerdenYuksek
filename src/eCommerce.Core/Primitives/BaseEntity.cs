namespace eCommerce.Core.Primitives;

public class BaseEntity
{
    #region Fields

    private readonly List<IDomainEvent> _domainEvents = new();

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

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion
}