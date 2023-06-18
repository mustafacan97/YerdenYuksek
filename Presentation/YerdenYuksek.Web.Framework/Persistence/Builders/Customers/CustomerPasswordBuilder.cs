using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public class CustomerPasswordBuilder : IEntityTypeConfiguration<CustomerPassword>
{
    public void Configure(EntityTypeBuilder<CustomerPassword> builder)
    {
        builder.ToTable("CustomerPassword");

        builder.HasKey(x => x.CustomerId);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);
    }
}
