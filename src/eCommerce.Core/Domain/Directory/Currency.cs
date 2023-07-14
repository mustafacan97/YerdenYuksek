using eCommerce.Core.Primitives;
using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Domain.Customers;

namespace eCommerce.Core.Domain.Directory;

public partial class Currency : SoftDeletedEntity, ILocalizedEntity
{
    public Currency()
    {
        Customers = new HashSet<Customer>();   
        Languages = new HashSet<Language>();
    }

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
}