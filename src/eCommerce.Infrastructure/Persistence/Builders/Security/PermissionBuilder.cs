using eCommerce.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public sealed class PermissionBuilder : IEntityTypeConfiguration<Permission>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permission");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(256);

        builder.Property(x => x.Category)
            .HasMaxLength(256);
    }

    #endregion
}
