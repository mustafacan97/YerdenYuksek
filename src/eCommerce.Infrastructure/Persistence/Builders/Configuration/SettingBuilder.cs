using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Primitives;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Configuration;

public sealed class SettingBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(Setting.Name)).AsString(256).NotNullable()
            .WithColumn(nameof(Setting.Value)).AsString(256);
    }

    #endregion
}
