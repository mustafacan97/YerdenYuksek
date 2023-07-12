using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Customers;

public class CustomerRole : BaseEntity
{
    #region Constructure and Destructure

    public CustomerRole()
    {
        Customers = new HashSet<Customer>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public ICollection<Customer> Customers { get; set; }

    #endregion
}