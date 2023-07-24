using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders;

public interface IEntityBuilder
{
    void MapEntity(CreateTableExpressionBuilder table);
}
