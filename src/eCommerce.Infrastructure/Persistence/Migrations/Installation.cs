using eCommerce.Core.Entities.Common;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Media;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator;
using System.Data;

namespace eCommerce.Infrastructure.Persistence.Migrations;

[Migration(24072023, "Create tables")]
public class Installation : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<Picture>();
        Create.TableFor<Currency>();
        Create.TableFor<Language>();
        Create.TableFor<Customer>();
        Create.TableFor<CustomerSecurity>();
        Create.TableFor<Country>();
        Create.TableFor<City>();
        Create.TableFor<Address>();

        // Karşılıklı ForeignKey olduğu için, Address tablosu oluştuktan sonra aşağıdakini eklemeliyim!
        Alter.Table(nameof(Customer))
            .AlterColumn(nameof(Customer.DefaultAddressId)).AsGuid().ForeignKey<Address>(onDelete: Rule.SetNull).Nullable();
    }
}
