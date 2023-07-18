using eCommerce.Core.Entities.Catalog;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Tax;

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
