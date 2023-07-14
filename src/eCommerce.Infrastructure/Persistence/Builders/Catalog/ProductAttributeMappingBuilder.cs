using eCommerce.Core.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public class ProductAttributeMappingBuilder : IEntityTypeConfiguration<ProductAttributeMapping>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ProductAttributeMapping> builder)
    {
        builder.ToTable("ProductAttributeMapping");

        builder.HasKey(q => q.Id);

        builder.HasMany(q => q.ProductAttributeValues)
            .WithOne()
            .HasForeignKey(q => q.ProductAttributeMappingId)
            .IsRequired();
    }

    #endregion
}
