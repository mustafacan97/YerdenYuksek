using eCommerce.Core.Entities.Security;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public class RoleBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(Role.Name)).AsString(256).NotNullable()
            .WithColumn(nameof(Role.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Role.Active)).AsBoolean().WithDefaultValue(true).NotNullable()
            .WithColumn(nameof(Role.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
