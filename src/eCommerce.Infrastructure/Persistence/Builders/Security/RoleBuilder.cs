using eCommerce.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Builders.Security;

public sealed class RoleBuilder : IEntityTypeConfiguration<Role>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(256);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasMany(x => x.Permissions)
            .WithMany(x => x.Roles)
            .UsingEntity(
                "RolePermissionMapping",
                l => l.HasOne(typeof(Permission)).WithMany().HasForeignKey("PermissionId").HasPrincipalKey(nameof(Permission.Id)),
                r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                j => j.HasKey("PermissionId", "RoleId"));

        builder.HasData(SeedCustomerRoles());
    }

    #endregion

    #region Methods

    private static IList<Role> SeedCustomerRoles()
    {
        var crAdministrators = new Role
        {
            Name = RoleDefaults.AdministratorsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crForumModerators = new Role
        {
            Name = RoleDefaults.ForumModeratorsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crRegistered = new Role
        {
            Name = RoleDefaults.RegisteredRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crGuests = new Role
        {
            Name = RoleDefaults.GuestsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crVendors = new Role
        {
            Name = RoleDefaults.VendorsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };

        var customerRoles = new List<Role> { crAdministrators, crForumModerators, crRegistered, crGuests, crVendors };

        return customerRoles;
    }

    #endregion
}
