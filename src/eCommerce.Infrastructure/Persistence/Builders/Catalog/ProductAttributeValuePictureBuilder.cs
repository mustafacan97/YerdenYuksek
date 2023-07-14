using eCommerce.Core.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public class ProductAttributeValuePictureBuilder : IEntityTypeConfiguration<ProductAttributeValuePicture>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ProductAttributeValuePicture> builder)
    {
        builder.ToTable("ProductAttributeValuePicture");

        builder.HasKey(q => new { q.ProductAttributeValueId, q.PictureId });

        builder.HasOne(q => q.ProductAttributeValue)
            .WithOne()
            .HasForeignKey<ProductAttributeValuePicture>(q => q.ProductAttributeValueId)
            .IsRequired();

        builder.HasOne(q => q.Picture)
            .WithOne()
            .HasForeignKey<ProductAttributeValuePicture>(q => q.PictureId)
            .IsRequired();
    }

    #endregion
}
