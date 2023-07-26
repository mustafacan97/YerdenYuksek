using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Messages;

public class EmailTemplateBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(EmailTemplate.Name)).AsString(256).NotNullable()
            .WithColumn(nameof(EmailTemplate.Bcc)).AsString(256).Nullable()
            .WithColumn(nameof(EmailTemplate.Subject)).AsString(512).NotNullable()
            .WithColumn(nameof(EmailTemplate.Body)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(EmailTemplate.EmailAccountId)).AsGuid().ForeignKey<EmailAccount>().NotNullable()
            .WithColumn(nameof(EmailTemplate.AttachedDownloadId)).AsInt16().Nullable()
            .WithColumn(nameof(EmailTemplate.AllowDirectReply)).AsBoolean().NotNullable()
            .WithColumn(nameof(EmailTemplate.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(EmailTemplate.Active)).AsBoolean().WithDefaultValue(true).NotNullable()
            .WithColumn(nameof(EmailTemplate.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
