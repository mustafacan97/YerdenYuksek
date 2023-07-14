using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Domain.Messages;
using eCommerce.Application.Services.Common;
using eCommerce.Core.Domain.Messages;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public sealed class MesasgeTemplateBuilder : IEntityTypeConfiguration<EmailTemplate>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.ToTable("MessageTemplate");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.Property(q => q.Bcc)
            .HasMaxLength(256);

        builder.Property(q => q.Subject)
            .HasMaxLength(512);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasData(SeedMessageTemplateData());
    }

    #endregion

    #region Methods

    private static IList<EmailTemplate> SeedMessageTemplateData()
    {
        var customerWelcomeMessage = new EmailTemplate
        {
            Id = Guid.NewGuid(),
            Name = EmailTemplateSystemNames.CustomerWelcomeMessage,
            Bcc = null,
            Subject = "Welcome to %Store.Name%",
            Body = $"We welcome you to <a href=\"%Store.URL%\"> %Store.Name%</a>.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}You can now take part in the various services we have to offer you. Some of these services include:{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Permanent Cart - Any products added to your online cart remain there until you remove them, or check them out.{Environment.NewLine}<br />{Environment.NewLine}Address Book - We can now deliver your products to another address other than yours! This is perfect to send birthday gifts direct to the birthday-person themselves.{Environment.NewLine}<br />{Environment.NewLine}Order History - View your history of purchases that you have made with us.{Environment.NewLine}<br />{Environment.NewLine}Products Reviews - Share your opinions on products with our other customers.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}For help with any of our online services, please email the store-owner: <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Note: This email address was provided on our registration page. If you own the email and did not register on our site, please send an email to <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.{Environment.NewLine}",
            EmailAccountId = CommonDefaults.DefaultEmailAccountId,
            CreatedOnUtc = DateTime.UtcNow,
            Active = true,
            Deleted = false
        };

        return new List<EmailTemplate> { customerWelcomeMessage };
    }

    #endregion
}