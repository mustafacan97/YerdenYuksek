using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Logging;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;
using System.Data;

namespace eCommerce.Infrastructure.Persistence.Builders.Logging;

public class LogBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(Log.ShortMessage)).AsString(256).NotNullable()
            .WithColumn(nameof(Log.FullMessage)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(Log.IpAddress)).AsString(32).Nullable()
            .WithColumn(nameof(Log.CustomerId)).AsGuid().ForeignKey<Customer>(onDelete: Rule.SetNull).Nullable()
            .WithColumn(nameof(Log.LogLevelId)).AsInt16().NotNullable()
            .WithColumn(nameof(Log.CreatedOnUtc)).AsCustomDateTime().NotNullable();
    }

    #endregion
}
