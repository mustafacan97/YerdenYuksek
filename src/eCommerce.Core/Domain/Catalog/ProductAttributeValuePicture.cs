using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Domain.Media;

namespace eCommerce.Core.Domain.Catalog;

public class ProductAttributeValuePicture : ILocalizedEntity
{
    public Guid ProductAttributeValueId { get; set; }

    public Guid PictureId { get; set; }

    public ProductAttributeValue ProductAttributeValue { get; set; }

    public Picture Picture { get; set; }
}
