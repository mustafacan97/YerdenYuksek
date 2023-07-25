using eCommerce.Core.Entities.Security;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public class PermissionRoleMappingBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(PermissionRoleMapping.RoleId)).AsGuid().ForeignKey<Role>().NotNullable()
            .WithColumn(nameof(PermissionRoleMapping.PermissionId)).AsGuid().ForeignKey<Permission>().NotNullable();
    }

    #endregion
}
