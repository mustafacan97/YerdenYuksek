using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Directory;

public sealed class OutboxMessageBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(OutboxMessage.Type)).AsString(256).NotNullable()
            .WithColumn(nameof(OutboxMessage.Content)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(OutboxMessage.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(OutboxMessage.ProcessedOnUtc)).AsCustomDateTime().Nullable()
            .WithColumn(nameof(OutboxMessage.Error)).AsString(512).Nullable();
    }

    #endregion
}
