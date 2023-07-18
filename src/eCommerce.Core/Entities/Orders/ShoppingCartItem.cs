using eCommerce.Core.Entities.Catalog;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Orders;

public class ShoppingCartItem : BaseEntity
{
    #region Public Properties

    public Guid CustomerId { get; set; }

    public Guid ProductId { get; set; }

    public int ShoppingCartTypeId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Product Product { get; set; }

    public ShoppingCartType ShoppingCartType
    {
        get => (ShoppingCartType)ShoppingCartTypeId;
        set => ShoppingCartTypeId = (int)value;
    }

    #endregion
}
