using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Common;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public class CustomerBuilder : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Username)
            .HasMaxLength(64);

        builder.Property(e => e.Email)
            .HasMaxLength(128);

        builder.Property(e => e.FirstName)
            .HasMaxLength(128);

        builder.Property(e => e.LastName)
            .HasMaxLength(128);

        builder.Property(e => e.Gender)
            .HasMaxLength(64);

        builder.Property(e => e.DateOfBirth)
            .HasPrecision(6);

        builder.Property(e => e.SystemName)
            .HasMaxLength(400);

        builder.Property(e => e.CannotLoginUntilDateUtc)
            .HasPrecision(6);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastLoginDateUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastActivityDateUtc)
            .HasPrecision(6);

        builder.HasMany(q => q.AllAddresses)
            .WithMany(q => q.AllCustomers)
            .UsingEntity(
                "CustomerAddressMapping",
                l => l.HasOne(typeof(Address)).WithMany().HasForeignKey("AddressId").HasPrincipalKey(nameof(Address.Id)),
                r => r.HasOne(typeof(Customer)).WithMany().HasForeignKey("CustomerId").HasPrincipalKey(nameof(Customer.Id)),
                j => j.HasKey("AddressId", "CustomerId"));
    }
}
