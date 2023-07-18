namespace eCommerce.Core.Entities.Orders;

public class OrderNote
{
    public Guid OrderId { get; set; }

    public string Note { get; set; }

    public bool DisplayToCustomer { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
