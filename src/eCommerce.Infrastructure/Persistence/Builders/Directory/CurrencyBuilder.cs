using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Persistence.DataProviders;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Directory;

public class CurrencyBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(Currency.Name)).AsString(64).NotNullable()
            .WithColumn(nameof(Currency.CurrencyCode)).AsString(4).NotNullable()
            .WithColumn(nameof(Currency.DisplayLocale)).AsString(8).Nullable()
            .WithColumn(nameof(Currency.CustomFormatting)).AsString(32).Nullable()
            .WithColumn(nameof(Currency.Rate)).AsDecimal(18,4).NotNullable()
            .WithColumn(nameof(Currency.DisplayOrder)).AsInt16().NotNullable()
            .WithColumn(nameof(Currency.RoundingTypeId)).AsInt16().NotNullable()
            .WithColumn(nameof(Currency.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Currency.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Currency.Deleted)).AsBoolean().NotNullable();
    }    

    #endregion
}
