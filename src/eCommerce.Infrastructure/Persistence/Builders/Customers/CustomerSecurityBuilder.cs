using eCommerce.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class CustomerSecurityBuilder : IEntityTypeConfiguration<CustomerSecurity>
{
    public void Configure(EntityTypeBuilder<CustomerSecurity> builder)
    {
        builder.ToTable("CustomerSecurity");

        builder.HasKey(x => x.CustomerId);

        builder.Property(q => q.PasswordSalt)
            .HasMaxLength(16);

        builder.Property(q => q.LastIpAddress)
            .HasMaxLength(64);

        builder.Property(q => q.CannotLoginUntilDateUtc)
            .HasPrecision(6);
    }
}
