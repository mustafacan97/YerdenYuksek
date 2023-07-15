using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Domain.Messages;
using Microsoft.Extensions.Configuration;

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
        IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var defaultEmail = new EmailAccount
        {
            Id = _configuration.GetValue<Guid>("DefaultValues:EmailAccountId"),
            Email = _configuration.GetValue<string>("DefaultValues:EmailAccount:Email"),
            Host = _configuration.GetValue<string>("DefaultValues:EmailAccount:Host"),
            Port = _configuration.GetValue<int>("DefaultValues:EmailAccount:Port"),
            Username = _configuration.GetValue<string>("DefaultValues:EmailAccount:Username"),
            Password = _configuration.GetValue<string>("DefaultValues:EmailAccount:Password"),
            EnableSsl = _configuration.GetValue<bool>("DefaultValues:EmailAccount:EnableSsl"),
            Active = true,
            Deleted = false
        };

        return new List<EmailAccount> { defaultEmail };
    }

    #endregion
}