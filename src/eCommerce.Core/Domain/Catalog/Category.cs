using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Domain.Media;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Catalog;

public class Category : SoftDeletedEntity, ILocalizedEntity
{
    #region Constructure and Destructure

    public Category()
    {
        Products = new HashSet<Product>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string PageSizeOptions { get; set; }

    public int SelectedPageSize { get; set; }

    public string Description { get; set; }

    public Guid? ParentCategoryId { get; set; }

    public Guid? PictureId { get; set; }

    public bool ShowOnHomepage { get; set; }

    public bool IncludeInTopMenu { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Category? ParentCategory { get; set; }

    public Picture? Picture { get; set; }

    public ICollection<Product> Products { get; set; }

    #endregion
}