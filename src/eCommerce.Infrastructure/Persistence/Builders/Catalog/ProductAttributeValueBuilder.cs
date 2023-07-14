using eCommerce.Core.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public class ProductAttributeValueBuilder : IEntityTypeConfiguration<ProductAttributeValue>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
    {
        builder.ToTable("ProductAttributeValue");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(512);

        builder.Property(q => q.ColorSquaresRgb)
            .HasMaxLength(64);
    }

    #endregion
}
