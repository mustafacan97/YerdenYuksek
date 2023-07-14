using eCommerce.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class AddressBuilder : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.FirstName)
            .HasMaxLength(128);

        builder.Property(q => q.LastName)
            .HasMaxLength(128);

        builder.Property(q => q.Email)
            .HasMaxLength(128);

        builder.Property(q => q.PhoneNumber)
            .HasMaxLength(12);

        builder.Property(q => q.ZipCode)
            .HasMaxLength(64);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(q => q.Active)
            .HasDefaultValue(true);

        builder.HasMany(q => q.BillingOrders)
            .WithOne()
            .HasForeignKey(q => q.BillingAddressId)
            .IsRequired();

        builder.HasMany(q => q.ShippingOrders)
            .WithOne()
            .HasForeignKey(q => q.ShippingAddressId)
            .IsRequired();
    }
}
