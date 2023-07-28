using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public class TokenBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(Token.Value)).AsString(256).NotNullable()
            .WithColumn(nameof(Token.CustomerId)).AsGuid().ForeignKey<Customer>().NotNullable()
            .WithColumn(nameof(Token.Status)).AsInt16().NotNullable()
            .WithColumn(nameof(Token.Type)).AsInt16().NotNullable()
            .WithColumn(nameof(Token.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Token.ExpiredOnUtc)).AsCustomDateTime().NotNullable();
    }

    #endregion
}
