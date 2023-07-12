using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Framework.Persistence.Builders;

public sealed class CustomerRoleBuilder : IEntityTypeConfiguration<CustomerRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<CustomerRole> builder)
    {
        builder.ToTable("CustomerRole");

        builder.HasKey(x => x.Id);

        builder.Property(q => q.Name)
            .HasMaxLength(255);

        builder.Property(q => q.Active)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasData(SeedCustomerRoles());
    }

    #endregion

    #region Methods

    private static IList<CustomerRole> SeedCustomerRoles()
    {
        var crAdministrators = new CustomerRole
        {
            Name = CustomerDefaults.AdministratorsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crForumModerators = new CustomerRole
        {
            Name = CustomerDefaults.ForumModeratorsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crRegistered = new CustomerRole
        {
            Name = CustomerDefaults.RegisteredRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crGuests = new CustomerRole
        {
            Name = CustomerDefaults.GuestsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var crVendors = new CustomerRole
        {
            Name = CustomerDefaults.VendorsRoleName,
            Active = true,
            Deleted = false,
            CreatedOnUtc = DateTime.UtcNow
        };
        var customerRoles = new List<CustomerRole>
            {
                crAdministrators,
                crForumModerators,
                crRegistered,
                crGuests,
                crVendors
            };

        return customerRoles;
    }

    #endregion
}
