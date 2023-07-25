using eCommerce.Core.Entities.Directory;
using eCommerce.Infrastructure.Persistence.DataProviders;
using FluentMigrator;

namespace eCommerce.Infrastructure.Persistence.Migrations.Installation;

[Migration(2507202302, "Seed primitives datas")]
public class SeedData : ForwardOnlyMigration
{
    #region Fields

    private readonly ICustomDataProvider _customDataProvider;

    #endregion

    #region Constructure and Destructure

    public SeedData(ICustomDataProvider customDataProvider)
    {
        _customDataProvider = customDataProvider;
    }

    #endregion

    #region Public Methods

    public override void Up()
    {
        SeedCurrencyData(_customDataProvider);
    }

    #endregion

    #region Methods

    private static void SeedCurrencyData(ICustomDataProvider _customDataProvider)
    {
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Name = "US Dollar",
            CurrencyCode = "USD",
            DisplayLocale = "en-US",
            CustomFormatting = string.Empty,
            Rate = 1,
            DisplayOrder = 1,
            RoundingTypeId = (int)RoundingType.Rounding001,
            CreatedOnUtc = DateTime.UtcNow,
            Active = true,
            Deleted = false
        };

        _customDataProvider.InsertEntity(currency);
    }

    #endregion
}
