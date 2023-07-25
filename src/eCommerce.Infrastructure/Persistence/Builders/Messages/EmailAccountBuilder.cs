using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Primitives;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Messages;

public class EmailAccountBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(EmailAccount.Email)).AsString(128).NotNullable()
            .WithColumn(nameof(EmailAccount.Host)).AsString(256).NotNullable()
            .WithColumn(nameof(EmailAccount.Port)).AsInt16().Nullable()
            .WithColumn(nameof(EmailAccount.Username)).AsString(128).NotNullable()
            .WithColumn(nameof(EmailAccount.Password)).AsString(128).NotNullable()
            .WithColumn(nameof(EmailAccount.PasswordSalt)).AsString(16).NotNullable()
            .WithColumn(nameof(EmailAccount.EnableSsl)).AsBoolean().NotNullable()
            .WithColumn(nameof(EmailAccount.Active)).AsBoolean().WithDefaultValue(true).NotNullable()
            .WithColumn(nameof(EmailAccount.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
