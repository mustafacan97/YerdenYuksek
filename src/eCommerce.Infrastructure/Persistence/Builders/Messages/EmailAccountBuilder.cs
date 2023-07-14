using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Domain.Messages;
using eCommerce.Core.Domain.Configuration.CustomSettings;
using eCommerce.Application.Services.Common;

namespace YerdenYuksek.Web.Framework.Persistence.Builders.Localization;

public sealed class EmailAccountBuilder : IEntityTypeConfiguration<EmailAccount>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<EmailAccount> builder)
    {
        builder.ToTable("EmailAccount");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Email)
            .HasMaxLength(128);

        builder.Property(e => e.DisplayName)
            .HasMaxLength(256);

        builder.Property(e => e.Host)
            .HasMaxLength(256);

        builder.Property(e => e.Username)
            .HasMaxLength(256);

        builder.Property(e => e.Password)
            .HasMaxLength(256);

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
            Email = "test@mail.com",
            DisplayName = "Store name",
            Host = "smtp.mail.com",
            Port = 25,
            Username = "123",
            Password = "123",
            EnableSsl = false,
            Active = true,
            Deleted = false
        };

        return new List<EmailAccount> { defaultEmail };
    }

    #endregion
}