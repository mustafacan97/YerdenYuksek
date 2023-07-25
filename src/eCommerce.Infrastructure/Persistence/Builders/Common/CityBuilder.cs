using eCommerce.Core.Entities.Common;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Common;

public class CityBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(City.Name)).AsString(128).NotNullable()
            .WithColumn(nameof(City.Abbreviation)).AsString(8).NotNullable()
            .WithColumn(nameof(City.CountryId)).AsGuid().ForeignKey<Country>().NotNullable()
            .WithColumn(nameof(City.DisplayOrder)).AsInt16().NotNullable()
            .WithColumn(nameof(City.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(City.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
