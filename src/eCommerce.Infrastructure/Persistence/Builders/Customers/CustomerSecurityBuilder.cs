using eCommerce.Core.Entities.Customers;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Customers;

public class CustomerSecurityBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerSecurity.CustomerId)).AsGuid().ForeignKey<Customer>().NotNullable().PrimaryKey()
            .WithColumn(nameof(CustomerSecurity.Password)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(CustomerSecurity.PasswordSalt)).AsString(16).NotNullable()
            .WithColumn(nameof(CustomerSecurity.LastIpAddress)).AsString(64)
            .WithColumn(nameof(CustomerSecurity.RequireReLogin)).AsBoolean().NotNullable()
            .WithColumn(nameof(CustomerSecurity.FailedLoginAttempts)).AsInt16().NotNullable()
            .WithColumn(nameof(CustomerSecurity.CannotLoginUntilDateUtc)).AsCustomDateTime();
    }

    #endregion
}
