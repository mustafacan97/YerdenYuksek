using eCommerce.Core.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public sealed class ProductBuilder : IEntityTypeConfiguration<Product>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(512);

        builder.Property(q => q.Sku)
            .HasMaxLength(512);

        builder.Property(q => q.Gtin)
            .HasMaxLength(512);

        builder.Property(q => q.RequiredProductIds)
            .HasMaxLength(1024);

        builder.Property(q => q.ShortDescription)
            .HasMaxLength(512);

        builder.Property(q => q.AdditionalShippingCharge)
            .HasPrecision(18, 4);

        builder.Property(q => q.ManufacturerPartNumber)
            .HasMaxLength(64);

        builder.Property(q => q.Price)
            .HasPrecision(18, 4);

        builder.Property(q => q.OldPrice)
            .HasPrecision(18, 4);

        builder.Property(q => q.ProductCost)
            .HasPrecision(18, 4);

        builder.Property(q => q.Weight)
            .HasPrecision(18, 4);

        builder.Property(q => q.Length)
            .HasPrecision(18, 4);

        builder.Property(q => q.Width)
            .HasPrecision(18, 4);

        builder.Property(q => q.Height)
            .HasPrecision(18, 4);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasMany(q => q.ProductAttributeMappings)
            .WithOne( q => q.Product)
            .HasForeignKey(q => q.ProductId)
            .IsRequired();

        builder.HasMany(q => q.Categories)
            .WithMany(q => q.Products)
            .UsingEntity(
                "ProductCategoryMapping",
                l => l.HasOne(typeof(Category)).WithMany().HasForeignKey("CategoryId").HasPrincipalKey(nameof(Category.Id)),
                r => r.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.Id)),
                j => j.HasKey("CategoryId", "ProductId"));
    }

    #endregion
}
