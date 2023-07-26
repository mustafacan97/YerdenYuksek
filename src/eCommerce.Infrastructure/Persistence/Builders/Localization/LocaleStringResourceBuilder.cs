using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public class LocaleStringResourceBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(LocaleStringResource.ResourceName)).AsString(256).NotNullable()
            .WithColumn(nameof(LocaleStringResource.ResourceValue)).AsString(short.MaxValue).NotNullable()
            .WithColumn(nameof(LocaleStringResource.LanguageId)).AsGuid().ForeignKey<Language>().NotNullable();
    }

    #endregion
}
