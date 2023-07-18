using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Orders;

public class OrderItem : BaseEntity
{
    public Guid ProductId { get; set; }

    public Guid OrderId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPriceInclTax { get; set; }

    public decimal UnitPriceExclTax { get; set; }

    public decimal PriceInclTax { get; set; }

    public decimal PriceExclTax { get; set; }

    public decimal OriginalProductCost { get; set; }
}
