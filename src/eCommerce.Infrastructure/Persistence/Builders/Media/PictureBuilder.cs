using eCommerce.Core.Domain.Customers;
using eCommerce.Core.Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Media;

public class PictureBuilder : IEntityTypeConfiguration<Picture>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        builder.ToTable("Picture");

        builder.HasKey(p => p.Id);

        builder.Property(q => q.MimeType)
            .HasMaxLength(16);

        builder.Property(q => q.SeoFilename)
            .HasMaxLength(128);

        builder.Property(q => q.AltAttribute)
            .HasMaxLength(128);

        builder.Property(q => q.TitleAttribute)
            .HasMaxLength(128);

        builder.Property(q => q.VirtualPath)
            .HasMaxLength(512);

        builder.Property(q => q.CreatedOnUtc)
            .HasPrecision(6);

        builder.Property(q => q.Active)
            .HasDefaultValue(true);

        builder.HasOne(q => q.Customer)
            .WithOne(q => q.Picture)
            .HasForeignKey<Customer>(q => q.PictureId)
            .IsRequired();
    }

    #endregion
}
