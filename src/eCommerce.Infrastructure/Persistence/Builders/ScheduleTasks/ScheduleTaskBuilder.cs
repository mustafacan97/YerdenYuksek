using eCommerce.Core.Entities.ScheduleTasks;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.ScheduleTasks;

public sealed class ScheduleTaskBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(ScheduleTask.Name)).AsString(256).NotNullable()
            .WithColumn(nameof(ScheduleTask.Seconds)).AsInt32().NotNullable()
            .WithColumn(nameof(ScheduleTask.Type)).AsString(512).NotNullable()
            .WithColumn(nameof(ScheduleTask.LastStartUtc)).AsCustomDateTime().Nullable()
            .WithColumn(nameof(ScheduleTask.LastEndUtc)).AsCustomDateTime().Nullable()
            .WithColumn(nameof(ScheduleTask.LastSuccessUtc)).AsCustomDateTime().Nullable()
            .WithColumn(nameof(ScheduleTask.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(ScheduleTask.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
