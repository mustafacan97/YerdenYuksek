using eCommerce.Core.Entities.Common;
using eCommerce.Core.Entities.Configuration;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Media;
using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Entities.ScheduleTasks;
using eCommerce.Core.Entities.Security;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator;
using System.Data;

namespace eCommerce.Infrastructure.Persistence.Migrations.Installation;

[Migration(2507202301, "Create tables")]
public class CreateTables : ForwardOnlyMigration
{
    #region Public Methods

    public override void Up()
    {
        Create.TableFor<Currency>();
        Create.TableFor<Picture>();
        Create.TableFor<Language>();
        Create.TableFor<Customer>();
        Create.TableFor<CustomerSecurity>();
        Create.TableFor<Country>();
        Create.TableFor<City>();
        Create.TableFor<Address>();
        Create.TableFor<Role>();
        Create.TableFor<Permission>();
        Create.TableFor<CustomerRoleMapping>();
        Create.TableFor<PermissionRoleMapping>();
        Create.TableFor<OutboxMessage>();
        Create.TableFor<ScheduleTask>();
        Create.TableFor<Setting>();
        Create.TableFor<EmailAccount>();
        Create.TableFor<QueuedEmail>();
        Create.TableFor<EmailTemplate>();

        // Karşılıklı ForeignKey olduğu için, Address tablosu oluştuktan sonra aşağıdakini eklemeliyim!
        Create.ForeignKey().FromTable(nameof(Customer)).ForeignColumn(nameof(Customer.DefaultAddressId))
                .ToTable(nameof(Address)).PrimaryColumn(nameof(Address.Id)).OnDelete(Rule.SetNull);
    }

    #endregion
}
