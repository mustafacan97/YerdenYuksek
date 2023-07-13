using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class CustomerPasswordBuilder : IEntityTypeConfiguration<CustomerSecurity>
{
    public void Configure(EntityTypeBuilder<CustomerSecurity> builder)
    {
        builder.ToTable("CustomerPassword");

        builder.HasKey(x => x.CustomerId);

        builder.Property(q => q.PasswordSalt)
            .HasMaxLength(16);
    }
}
