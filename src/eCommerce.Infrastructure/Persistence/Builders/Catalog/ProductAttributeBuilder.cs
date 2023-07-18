using eCommerce.Core.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public class ProductAttributeBuilder : IEntityTypeConfiguration<ProductAttribute>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable("ProductAttribute");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.HasMany(q => q.ProductAttributeMappings)
            .WithOne(q => q.ProductAttribute)
            .HasForeignKey(q => q.ProductAttributeId)
            .IsRequired();
    }

    #endregion
}
