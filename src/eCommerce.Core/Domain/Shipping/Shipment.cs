using eCommerce.Core.Domain.Orders;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Shipping;

public class Shipment : BaseEntity
{
    #region Constructure and Destructure

    public Shipment()
    {
        ShipmentItems = new HashSet<ShipmentItem>();
    }

    #endregion

    #region Public Properties

    public Guid OrderId { get; set; }

    public string TrackingNumber { get; set; }

    public decimal? TotalWeight { get; set; }

    public DateTime? ShippedDateUtc { get; set; }

    public DateTime? DeliveryDateUtc { get; set; }

    public string? AdminComment { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Order Order { get; set; }

    public ICollection<ShipmentItem> ShipmentItems { get; set; }

    #endregion
}