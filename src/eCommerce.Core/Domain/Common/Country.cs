using eCommerce.Core.Domain.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Common;

public class Country : SoftDeletedEntity, ILocalizedEntity
{
    #region Constructure and Destructure

    public Country()
    {
        Addresses = new HashSet<Address>();
        Cities = new HashSet<City>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string TwoLetterIsoCode { get; set; }

    public bool AllowsBilling { get; set; }

    public bool AllowsShipping { get; set; }

    public int CountryCode { get; set; }

    public int DisplayOrder { get; set; }

    public ICollection<Address> Addresses { get; set; }

    public ICollection<City> Cities { get; set; }

    #endregion
}