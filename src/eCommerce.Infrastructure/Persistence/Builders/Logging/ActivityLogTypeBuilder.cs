using eCommerce.Core.Entities.Logging;
using eCommerce.Core.Primitives;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Logging;

public class ActivityLogTypeBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(ActivityLogType.SystemKeyword)).AsString(128).NotNullable()
            .WithColumn(nameof(ActivityLogType.Name)).AsString(256).NotNullable();
    }

    #endregion
}
