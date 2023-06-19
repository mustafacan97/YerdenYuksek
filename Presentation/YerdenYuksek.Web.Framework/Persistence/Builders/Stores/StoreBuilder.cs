using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Stores;

namespace eCommerce.Framework.Persistence.Builders;

public class StoreBuilder : IEntityTypeConfiguration<Store>
{
    #region Static Fields and Constants

    public static readonly Guid DefaultStoreId = Guid.NewGuid();

    #endregion


    #region Public Methods

    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("Store");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(e => e.Url)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(e => e.Hosts)
            .HasMaxLength(1000);

        builder.Property(e => e.CompanyName)
            .HasMaxLength(1000);

        builder.Property(e => e.CompanyAddress)
            .HasMaxLength(1000);

        builder.Property(e => e.CompanyPhoneNumber)
            .HasMaxLength(1000);

        builder.Property(e => e.CompanyVat)
            .HasMaxLength(1000);

        builder.HasMany(q => q.RegisteredCustomers)
            .WithOne()
            .HasForeignKey(q => q.RegisteredInStoreId)
            .IsRequired();

        builder.HasMany(q => q.Settings)
            .WithOne()
            .HasForeignKey(q => q.StoreId)
            .IsRequired();

        builder.HasData(new Store
        {
            Id = DefaultStoreId,
            Name = "Yerden Yuksek Store",
            DefaultTitle = "Yerden Yuksek",
            DefaultMetaKeywords = string.Empty,
            DefaultMetaDescription = string.Empty,
            HomepageTitle = "Home page title",
            HomepageDescription = "Home page description",
            Url = string.Empty,
            SslEnabled = false,
            Hosts = "yourstore.com,www.yourstore.com",
            DisplayOrder = 1,
            CompanyName = "Your company name",
            CompanyAddress = "your company country, state, zip, street, etc",
            CompanyPhoneNumber = "(123) 456-78901",
            CompanyVat = string.Empty
        });
    }

    #endregion
}
