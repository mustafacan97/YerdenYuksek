using eCommerce.Core.Entities.Tax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Tax;

public class TaxCategoryBuilder : IEntityTypeConfiguration<TaxCategory>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<TaxCategory> builder)
    {
        builder.ToTable("TaxCategory");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.HasMany(q => q.Products)
            .WithOne(q => q.TaxCategory)
            .HasForeignKey(q => q.TaxCategoryId)
            .IsRequired();
    }

    #endregion
}
