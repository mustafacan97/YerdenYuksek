using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Domain.Configuration;
using eCommerce.Core.Domain.Localization;
using YerdenYuksek.Core.Domain.Logging;
using eCommerce.Core.Domain.Messages;
using eCommerce.Core.Domain.Security;
using eCommerce.Core.Domain.Customers;
using eCommerce.Core.Domain.Logging;
using eCommerce.Core.Domain.Common;

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

    public virtual DbSet<ActivityLog> ActivityLog { get; set; }

    public virtual DbSet<ActivityLogType> ActivityLogType { get; set; }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<CustomerSecurity> CustomerSecurity { get; set; }    

    public virtual DbSet<EmailAccount> EmailAccount { get; set; }

    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<LocaleStringResource> LocaleStringResource { get; set; }

    public virtual DbSet<Log> Log { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }

    public virtual DbSet<Permission> Permission { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Setting> Setting { get; set; }

    #endregion
}