using eCommerce.Core.Primitives;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Localization;

namespace eCommerce.Core.Entities.Directory;

public partial class Currency : SoftDeletedEntity, ILocalizedEntity
{
    #region Constructure and Destructure

    public Currency()
    {
        Customers = new HashSet<Customer>();
        Languages = new HashSet<Language>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string CurrencyCode { get; set; }

    public string? DisplayLocale { get; set; }

    public string? CustomFormatting { get; set; }

    public decimal Rate { get; set; }

    public int DisplayOrder { get; set; }

    public int RoundingTypeId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public ICollection<Customer> Customers { get; set; }

    public ICollection<Language> Languages { get; set; }

    public RoundingType RoundingType
    {
        get => (RoundingType)RoundingTypeId;
        set => RoundingTypeId = (int)value;
    }

    #endregion
}