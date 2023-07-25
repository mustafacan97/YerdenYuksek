using eCommerce.Core.Entities.Common;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Common;

public class CountryBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Country.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(Country.Name)).AsString(128).NotNullable()
            .WithColumn(nameof(Country.TwoLetterIsoCode)).AsString(2).NotNullable()
            .WithColumn(nameof(Country.AllowsBilling)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Country.AllowsShipping)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Country.CountryCode)).AsInt16().NotNullable()
            .WithColumn(nameof(Country.DisplayOrder)).AsInt16().NotNullable()
            .WithColumn(nameof(Country.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Country.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
