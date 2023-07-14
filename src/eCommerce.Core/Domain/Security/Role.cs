using eCommerce.Core.Domain.Customers;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Security;

public class Role : SoftDeletedEntity
{
    #region Constructure and Destructure

    public Role()
    {
        Permissions = new HashSet<Permission>();
        Customers = new HashSet<Customer>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public ICollection<Permission> Permissions { get; set; }

    public ICollection<Customer> Customers { get; set; }

    #endregion
}