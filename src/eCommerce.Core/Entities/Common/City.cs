using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Entities.Common;

public class City : SoftDeletedEntity, ILocalizedEntity
{
    #region Constructure ande Destructure

    public City()
    {
        Addresses = new HashSet<Address>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public Guid CountryId { get; set; }

    public int DisplayOrder { get; set; }

    public ICollection<Address> Addresses { get; set; }

    #endregion
}
