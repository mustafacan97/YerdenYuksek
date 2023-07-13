using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class CustomerBuilder : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.HasKey(x => x.Id);

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

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastLoginDateUtc)
            .HasPrecision(6);

        builder.Property(e => e.LastActivityDateUtc)
            .HasPrecision(6);

        builder.HasOne(q => q.CustomerSecurity)
            .WithOne()
            .HasForeignKey<CustomerSecurity>(q => q.CustomerId)
            .IsRequired();

        builder.HasMany(q => q.Addresses)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired();

        builder.HasMany(q => q.Logs)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired();

        builder.HasMany(q => q.ActivityLogs)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired();

        builder.HasMany(q => q.CustomerRoles)
            .WithMany(q => q.Customers)
            .UsingEntity(
                "CustomerRoleMapping",
                l => l.HasOne(typeof(CustomerRole)).WithMany().HasForeignKey("CustomerRoleId").HasPrincipalKey(nameof(CustomerRole.Id)),
                r => r.HasOne(typeof(Customer)).WithMany().HasForeignKey("CustomerId").HasPrincipalKey(nameof(Customer.Id)),
                j => j.HasKey("CustomerRoleId", "CustomerId"));
    }
}
