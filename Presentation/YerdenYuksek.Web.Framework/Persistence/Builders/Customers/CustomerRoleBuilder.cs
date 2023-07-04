using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class CustomerRoleBuilder : IEntityTypeConfiguration<CustomerRole>
{
    public void Configure(EntityTypeBuilder<CustomerRole> builder)
    {
        builder.ToTable("CustomerRole");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(255);

        builder.Property(q => q.Active)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);
    }
}
