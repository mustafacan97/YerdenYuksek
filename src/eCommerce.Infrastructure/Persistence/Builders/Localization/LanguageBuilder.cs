using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public class LanguageBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(Language.Name)).AsString(64).NotNullable()
            .WithColumn(nameof(Language.LanguageCulture)).AsString(8).NotNullable()
            .WithColumn(nameof(Language.UniqueSeoCode)).AsString(2).Nullable()
            .WithColumn(nameof(Language.FlagImageFileName)).AsString(16).Nullable()
            .WithColumn(nameof(Language.DefaultCurrencyId)).AsGuid().ForeignKey<Currency>().NotNullable()
            .WithColumn(nameof(Language.IsDefaultLanguage)).AsBoolean().NotNullable()
            .WithColumn(nameof(Language.DisplayOrder)).AsInt16().NotNullable()
            .WithColumn(nameof(Language.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Language.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Language.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
