using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Domain.Shipping;
using eCommerce.Core.Domain.Tax;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Catalog;

public class Product : SoftDeletedEntity, ILocalizedEntity
{
    #region Constructure and Destructure

    public Product()
    {
        ProductAttributeMappings = new HashSet<ProductAttributeMapping>();
        Categories = new HashSet<Category>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string Sku { get; set; }

    public string Gtin { get; set; }

    public bool RequireOtherProducts { get; set; }

    public string RequiredProductIds { get; set; }

    public string AllowedQuantities { get; set; }

    public string ShortDescription { get; set; }

    public string FullDescription { get; set; }

    public bool ShowOnHomepage { get; set; }

    public bool AllowCustomerReviews { get; set; }

    public int ApprovedRatingSum { get; set; }

    public int NotApprovedRatingSum { get; set; }

    public int ApprovedTotalReviews { get; set; }

    public int NotApprovedTotalReviews { get; set; }

    public bool IsShipEnabled { get; set; }

    public bool IsFreeShipping { get; set; }

    public decimal AdditionalShippingCharge { get; set; }

    public Guid ShipmentDeliveryDateId { get; set; }

    public bool IsTaxExempt { get; set; }

    public Guid TaxCategoryId { get; set; }    

    public Guid ManufacturerId { get; set; }

    public string? ManufacturerPartNumber { get; set; }

    public int StockQuantity { get; set; }

    public int OrderMinimumQuantity { get; set; }

    public int OrderMaximumQuantity { get; set; }

    public bool NotReturnable { get; set; }

    public bool DisableBuyButton { get; set; }

    public decimal Price { get; set; }

    public decimal OldPrice { get; set; }

    public decimal ProductCost { get; set; }

    public bool MarkAsNew { get; set; }

    public decimal Weight { get; set; }

    public decimal Length { get; set; }

    public decimal Width { get; set; }

    public decimal Height { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public ShipmentDeliveryDate ShipmentDeliveryDate { get; set; }

    public Manufacturer Manufacturer { get; set; }

    public TaxCategory TaxCategory { get; set; }

    public ICollection<ProductAttributeMapping> ProductAttributeMappings { get; set; }

    public ICollection<Category> Categories { get; set; }

    #endregion
}