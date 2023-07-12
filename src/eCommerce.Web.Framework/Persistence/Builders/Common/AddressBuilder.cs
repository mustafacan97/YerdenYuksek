using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Common;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class AddressBuilder : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(q => q.LastName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(q => q.Email)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(q => q.PhoneNumber)
            .HasMaxLength(12)
            .IsRequired();

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);
    }
}
