using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Stores;

namespace eCommerce.Framework.Persistence.Builders;

public class StoreBuilder : IEntityTypeConfiguration<Store>
{
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

        builder.HasMany(q => q.AllRegisteredCustomers)
            .WithOne()
            .HasForeignKey(q => q.RegisteredInStoreId)
            .IsRequired();
    }

    #endregion
}
