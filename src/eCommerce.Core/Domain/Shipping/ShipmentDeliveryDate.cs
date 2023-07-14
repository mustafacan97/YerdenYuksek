using eCommerce.Core.Domain.Catalog;
using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Shipping;

public class ShipmentDeliveryDate : BaseEntity, ILocalizedEntity
{
    #region Constructure and Destructure

    public ShipmentDeliveryDate()
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