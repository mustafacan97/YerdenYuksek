using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Security;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public class CustomerRoleMappingBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerRoleMapping.CustomerId)).AsGuid().ForeignKey<Customer>().NotNullable()
            .WithColumn(nameof(CustomerRoleMapping.RoleId)).AsGuid().ForeignKey<Role>().NotNullable();
    }

    #endregion
}
