using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Logging;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Logging;

public class ActivityLogBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(ActivityLog.ActivityLogTypeId)).AsGuid().ForeignKey<ActivityLogType>().NotNullable()
            .WithColumn(nameof(ActivityLog.CustomerId)).AsGuid().ForeignKey<Customer>().NotNullable()
            .WithColumn(nameof(ActivityLog.EntityName)).AsString(256).Nullable()
            .WithColumn(nameof(ActivityLog.EntityId)).AsGuid().Nullable()
            .WithColumn(nameof(ActivityLog.Comment)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(ActivityLog.CreatedOnUtc)).AsCustomDateTime().NotNullable();
    }

    #endregion
}
