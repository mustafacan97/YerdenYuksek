using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Orders;

public class ReturnRequest : BaseEntity
{
    public string ReasonForReturn { get; set; }

    public string RequestedAction { get; set; }

    public Guid OrderItemId { get; set; }

    public int Quantity { get; set; }

    public string CustomerComments { get; set; }

    public int ReturnRequestStatusId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public OrderItem OrderItem { get; set; }
    
    public ReturnRequestStatus ReturnRequestStatus
    {
        get => (ReturnRequestStatus)ReturnRequestStatusId;
        set => ReturnRequestStatusId = (int)value;
    }
}
