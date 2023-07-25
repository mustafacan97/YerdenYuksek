using eCommerce.Core.Entities.Catalog;
using eCommerce.Infrastructure.Persistence.Builders.Directory;
using eCommerce.Infrastructure.Persistence.DataProviders;
using FluentMigrator;
using LinqToDB.Mapping;

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
        CurrencyBuilder.SeedData(_customDataProvider);
    }

    #endregion
}
