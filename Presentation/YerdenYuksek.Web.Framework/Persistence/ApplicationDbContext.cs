using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Core.Domain.Common;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Infrastructure.Persistence.Primitives;

public class ApplicationDbContext : DbContext
{
    #region Constructure and Destructure

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #endregion

    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    #endregion

    #region Public Properties

    public virtual DbSet<Address> Address { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<CustomerPassword> CustomerPassword { get; set; }

    public virtual DbSet<CustomerAddressMapping> CustomerAddressMapping { get; set; }    

    #endregion
}