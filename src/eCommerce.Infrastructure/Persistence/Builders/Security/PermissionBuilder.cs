using eCommerce.Core.Entities.Security;
using eCommerce.Core.Primitives;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public class PermissionBuilder : IEntityBuilder
{
    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(Permission.Name)).AsString(256).NotNullable()
            .WithColumn(nameof(Permission.Category)).AsString(256).NotNullable();
    }
}
