using eCommerce.Core.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Catalog;

public class CategoryBuilder : IEntityTypeConfiguration<Category>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.Property(q => q.PageSizeOptions)
            .HasMaxLength(128);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasOne(q => q.ParentCategory)
            .WithOne()
            .HasForeignKey<Category>(q => q.ParentCategoryId)
            .IsRequired(false);
    }

    #endregion
}
