using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Catalog;

public class ProductAttributeValue : BaseEntity, ILocalizedEntity
{
    #region Public Properties

    public string Name { get; set; }

    public Guid ProductAttributeMappingId { get; set; }

    public Guid ImageSquaresPictureId { get; set; }

    public string ColorSquaresRgb { get; set; }

    public int Quantity { get; set; }

    public bool IsPreSelected { get; set; }

    public int DisplayOrder { get; set; }

    #endregion
}
