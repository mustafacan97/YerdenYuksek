using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Domain.Configuration;
using eCommerce.Core.Domain.Localization;
using YerdenYuksek.Core.Domain.Logging;
using eCommerce.Core.Domain.Messages;
using eCommerce.Core.Domain.Security;
using eCommerce.Core.Domain.Customers;
using eCommerce.Core.Domain.Logging;
using eCommerce.Core.Domain.Common;
using eCommerce.Core.Domain.Directory;
using eCommerce.Core.Domain.Media;
using eCommerce.Core.Domain.Orders;
using eCommerce.Core.Domain.Shipping;
using eCommerce.Core.Domain.Catalog;
using eCommerce.Core.Domain.Tax;
using eCommerce.Core.Domain.ScheduleTasks;

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

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<Currency> Currency { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<CustomerSecurity> CustomerSecurity { get; set; }    

    public virtual DbSet<EmailAccount> EmailAccount { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }

    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<LocaleStringResource> LocaleStringResource { get; set; }

    public virtual DbSet<LocalizedProperty> LocalizedProperty { get; set; }

    public virtual DbSet<Log> Log { get; set; }

    public virtual DbSet<Manufacturer> Manufacturer { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderItem> OrderItem { get; set; }

    public virtual DbSet<OrderNote> OrderNote { get; set; }

    public virtual DbSet<Permission> Permission { get; set; }

    public virtual DbSet<Picture> Picture { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttribute { get; set; }

    public virtual DbSet<ProductAttributeMapping> ProductAttributeMapping { get; set; }

    public virtual DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }

    public virtual DbSet<ProductAttributeValuePicture> ProductAttributeValuePicture { get; set; }

    public virtual DbSet<QueuedEmail> QueuedEmail { get; set; }

    public virtual DbSet<ReturnRequest> ReturnRequest { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<ScheduleTask> ScheduleTask { get; set; }

    public virtual DbSet<Setting> Setting { get; set; }

    public virtual DbSet<Shipment> Shipment { get; set; }

    public virtual DbSet<ShipmentDeliveryDate> ShipmentDeliveryDate { get; set; }

    public virtual DbSet<ShipmentItem> ShipmentItem { get; set; }

    public virtual DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }

    public virtual DbSet<TaxCategory> TaxCategory { get; set; }

    #endregion
}