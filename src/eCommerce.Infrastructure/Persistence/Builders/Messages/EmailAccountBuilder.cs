using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using eCommerce.Application.Services.Common;
using eCommerce.Core.Domain.Messages;

namespace eCommerce.Infrastructure.Persistence.Builders.Messages;

public sealed class EmailAccountBuilder : IEntityTypeConfiguration<EmailAccount>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<EmailAccount> builder)
    {
        builder.ToTable("EmailAccount");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Email)
            .HasMaxLength(128);

        builder.Property(e => e.Host)
            .HasMaxLength(256);

        builder.Property(e => e.Username)
            .HasMaxLength(128);

        builder.Property(e => e.Password)
            .HasMaxLength(128);

        builder.HasMany(q => q.QueuedEmails)
            .WithOne()
            .HasForeignKey(q => q.EmailAccountId)
            .IsRequired();

        builder.HasMany(q => q.EmailTemplates)
            .WithOne()
            .HasForeignKey(q => q.EmailAccountId)
            .IsRequired();

        builder.HasData(SeedEmailAccountData());
    }

    #endregion

    #region Methods

    private static IList<EmailAccount> SeedEmailAccountData()
    {
        var defaultEmail = new EmailAccount
        {
            Id = CommonDefaults.DefaultEmailAccountId,
            Email = "info@mustafacan.co",
            Host = "mustafacan.co",
            Port = 465,
            Username = "info@mustafacan.co",
            Password = "eCommerce.2023!",
            EnableSsl = true,
            Active = true,
            Deleted = false
        };

        return new List<EmailAccount> { defaultEmail };
    }

    #endregion
}