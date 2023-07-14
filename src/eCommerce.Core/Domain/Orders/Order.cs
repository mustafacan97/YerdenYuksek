using eCommerce.Core.Domain.Payments;
using eCommerce.Core.Domain.Shipping;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Orders;

public class Order : SoftDeletedEntity
{
    #region Constructure and Destructure

    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
        Shipments = new HashSet<Shipment>();
    }

    #endregion

    #region Public Properties

    public Guid CustomerId { get; set; }

    public Guid BillingAddressId { get; set; }

    public Guid ShippingAddressId { get; set; }

    public int OrderStatusId { get; set; }

    public int ShippingStatusId { get; set; }

    public int PaymentStatusId { get; set; }

    public string CurrencyCode { get; set; }

    public decimal CurrencyRate { get; set; }

    public decimal SubtotalInclTax { get; set; }

    public decimal SubtotalExclTax { get; set; }

    public decimal ShippingInclTax { get; set; }

    public decimal ShippingExclTax { get; set; }

    public decimal PaymentMethodAdditionalFeeInclTax { get; set; }

    public decimal PaymentMethodAdditionalFeeExclTax { get; set; }

    public string TaxRates { get; set; }

    public decimal Tax { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }

    public decimal RefundedAmount { get; set; }

    public string IpAddress { get; set; }    

    public DateTime? PaidDateUtc { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public OrderNote? OrderNote { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public ICollection<Shipment> Shipments { get; set; }

    #endregion

    #region Custom properties

    public OrderStatus OrderStatus
    {
        get => (OrderStatus)OrderStatusId;
        set => OrderStatusId = (int)value;
    }

    public PaymentStatus PaymentStatus
    {
        get => (PaymentStatus)PaymentStatusId;
        set => PaymentStatusId = (int)value;
    }

    public ShippingStatus ShippingStatus
    {
        get => (ShippingStatus)ShippingStatusId;
        set => ShippingStatusId = (int)value;
    }

    #endregion
}