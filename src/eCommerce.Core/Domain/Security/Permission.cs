using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Security;

public class Permission : BaseEntity
{
    #region Constructure and Destructure

    public Permission()
    {
        Roles = new HashSet<Role>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string Category { get; set; }

    public ICollection<Role> Roles { get; set; }

    #endregion
}
