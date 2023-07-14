using eCommerce.Core.Domain.Common;
using eCommerce.Core.Domain.Customers;
using eCommerce.Core.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Customers;

public sealed class CustomerBuilder : IEntityTypeConfiguration<Customer>
{
    #region Public Methods

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

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(12);

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

        builder.HasMany(q => q.Logs)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired(false);

        builder.HasMany(q => q.ActivityLogs)
            .WithOne()
            .HasForeignKey(x => x.CustomerId)
            .IsRequired();

        builder.HasOne(q => q.DefaultAddress)
            .WithOne()
            .HasForeignKey<Customer>(q => q.DefaultAddressId)
            .IsRequired(false);

        builder.HasMany(q => q.Addresses)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired();

        builder.HasMany(q => q.CustomerRoles)
            .WithMany(q => q.Customers)
            .UsingEntity(
                "CustomerRoleMapping",
                l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                r => r.HasOne(typeof(Customer)).WithMany().HasForeignKey("CustomerId").HasPrincipalKey(nameof(Customer.Id)),
                j => j.HasKey("RoleId", "CustomerId"));

        builder.HasMany(q => q.Orders)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired();

        builder.HasMany(q => q.ShoppingCartItems)
            .WithOne()
            .HasForeignKey(q => q.CustomerId)
            .IsRequired();
    }

    #endregion
}
