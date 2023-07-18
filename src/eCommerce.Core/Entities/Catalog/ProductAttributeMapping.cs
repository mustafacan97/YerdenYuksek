using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Catalog;

public class ProductAttributeMapping : BaseEntity, ILocalizedEntity
{
    #region Constructure and Destructur

    public ProductAttributeMapping()
    {
        ProductAttributeValues = new HashSet<ProductAttributeValue>();
    }

    #endregion

    #region Public Properties

    public Guid ProductAttributeId { get; set; }

    public Guid ProductId { get; set; }

    public bool IsRequired { get; set; }

    public int AttributeControlTypeId { get; set; }

    public int DisplayOrder { get; set; }

    public string DefaultValue { get; set; }

    public ProductAttribute ProductAttribute { get; set; }

    public Product Product { get; set; }

    public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }

    public AttributeControlType AttributeControlType
    {
        get => (AttributeControlType)AttributeControlTypeId;
        set => AttributeControlTypeId = (int)value;
    }

    #endregion
}