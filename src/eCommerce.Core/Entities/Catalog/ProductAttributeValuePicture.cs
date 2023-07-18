using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Media;

namespace eCommerce.Core.Entities.Catalog;

public class ProductAttributeValuePicture : ILocalizedEntity
{
    public Guid ProductAttributeValueId { get; set; }

    public Guid PictureId { get; set; }

    public ProductAttributeValue ProductAttributeValue { get; set; }

    public Picture Picture { get; set; }
}
