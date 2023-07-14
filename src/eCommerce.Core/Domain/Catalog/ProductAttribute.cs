using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Catalog;

public class ProductAttribute : BaseEntity, ILocalizedEntity
{
    public ProductAttribute()
    {
        ProductAttributeMappings = new HashSet<ProductAttributeMapping>();    
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public ICollection<ProductAttributeMapping> ProductAttributeMappings { get; set; }
}
