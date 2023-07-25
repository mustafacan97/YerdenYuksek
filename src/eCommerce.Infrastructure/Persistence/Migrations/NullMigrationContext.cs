using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace eCommerce.Infrastructure.Persistence.Migrations;

public class NullMigrationContext : IMigrationContext
{
    public IServiceProvider ServiceProvider { get; set; }

    public ICollection<IMigrationExpression> Expressions { get; set; } = new List<IMigrationExpression>();

    public IQuerySchema QuerySchema { get; set; }

    public IAssemblyCollection MigrationAssemblies { get; set; }

    public object ApplicationContext { get; set; }

    public string Connection { get; set; }
}