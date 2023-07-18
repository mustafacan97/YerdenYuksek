using eCommerce.Core.Entities.Orders;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Shipping;

public class ShipmentItem : BaseEntity
{
    public Guid ShipmentId { get; set; }

    public Guid OrderItemId { get; set; }

    public int Quantity { get; set; }

    public OrderItem OrderItem { get; set; }
}