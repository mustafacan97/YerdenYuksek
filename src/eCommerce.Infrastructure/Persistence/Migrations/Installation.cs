using eCommerce.Core.Entities.Customers;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator;

namespace eCommerce.Infrastructure.Persistence.Migrations;

[Migration(1, "Bu bir denemedir.")]
public class Installation : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<Customer>();
    }
}
