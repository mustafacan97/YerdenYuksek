using eCommerce.Core.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Messages;

internal class QueuedEmailBuilder : IEntityTypeConfiguration<QueuedEmail>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<QueuedEmail> builder)
    {
        builder.ToTable("QueuedEmail");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.From)
            .HasMaxLength(128);

        builder.Property(e => e.FromName)
            .HasMaxLength(128);

        builder.Property(e => e.To)
            .HasMaxLength(128);

        builder.Property(e => e.ToName)
            .HasMaxLength(128);

        builder.Property(e => e.ReplyTo)
            .HasMaxLength(128);

        builder.Property(e => e.ReplyToName)
            .HasMaxLength(128);

        builder.Property(e => e.Subject)
            .HasMaxLength(1024);

        builder.Property(e => e.CC)
            .HasMaxLength(512);

        builder.Property(e => e.Bcc)
            .HasMaxLength(512);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(e => e.SentOnUtc)
            .HasPrecision(6);
    }

    #endregion
}
