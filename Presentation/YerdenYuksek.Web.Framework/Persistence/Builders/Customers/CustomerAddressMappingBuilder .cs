using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public class CustomerAddressMappingBuilder : IEntityTypeConfiguration<CustomerAddressMapping>
{
    public void Configure(EntityTypeBuilder<CustomerAddressMapping> builder)
    {
        builder.ToTable("CustomerAddressMapping");

        builder.HasKey(q => new { q.CustomerId, q.AddressId });
    }
}
