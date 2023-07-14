using eCommerce.Core.Domain.Catalog;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Tax;

public class TaxCategory : BaseEntity
{
    #region Constructure and Destructure

    public TaxCategory()
    {
        Products = new HashSet<Product>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public int DisplayOrder { get; set; }

    public ICollection<Product> Products { get; set; }

    #endregion
}
