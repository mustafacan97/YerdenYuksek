using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public class LocalizedPropertyBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(LocalizedProperty.LocaleKeyGroup)).AsString(256).NotNullable()
            .WithColumn(nameof(LocalizedProperty.LocaleKey)).AsString(256).NotNullable()
            .WithColumn(nameof(LocalizedProperty.LocaleValue)).AsString(short.MaxValue).NotNullable()
            .WithColumn(nameof(LocalizedProperty.EntityId)).AsGuid().NotNullable()
            .WithColumn(nameof(LocalizedProperty.LanguageId)).AsGuid().ForeignKey<Language>().NotNullable();
    }

    #endregion
}
