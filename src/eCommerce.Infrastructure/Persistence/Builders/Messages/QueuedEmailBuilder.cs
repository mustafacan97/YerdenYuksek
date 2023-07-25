using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Messages;

public class QueuedEmailBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(nameof(QueuedEmail.From)).AsString(128).NotNullable()
            .WithColumn(nameof(QueuedEmail.FromName)).AsString(128).NotNullable()
            .WithColumn(nameof(QueuedEmail.To)).AsString(128).NotNullable()
            .WithColumn(nameof(QueuedEmail.ToName)).AsString(128).NotNullable()
            .WithColumn(nameof(QueuedEmail.ReplyTo)).AsString(128).Nullable()
            .WithColumn(nameof(QueuedEmail.ReplyToName)).AsString(128).Nullable()
            .WithColumn(nameof(QueuedEmail.Subject)).AsString(512).NotNullable()
            .WithColumn(nameof(QueuedEmail.CC)).AsString(512).Nullable()
            .WithColumn(nameof(QueuedEmail.Bcc)).AsString(512).Nullable()
            .WithColumn(nameof(QueuedEmail.Body)).AsString().NotNullable()
            .WithColumn(nameof(QueuedEmail.EmailAccountId)).AsGuid().ForeignKey<EmailAccount>().NotNullable()
            .WithColumn(nameof(QueuedEmail.PriorityId)).AsInt16().NotNullable()
            .WithColumn(nameof(QueuedEmail.AttachmentFilePath)).AsString().Nullable()
            .WithColumn(nameof(QueuedEmail.AttachmentFileName)).AsString().Nullable()
            .WithColumn(nameof(QueuedEmail.AttachedDownloadId)).AsInt16().Nullable()
            .WithColumn(nameof(QueuedEmail.SentTries)).AsInt16().NotNullable()
            .WithColumn(nameof(QueuedEmail.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(QueuedEmail.SentOnUtc)).AsCustomDateTime().Nullable();
    }

    #endregion
}
