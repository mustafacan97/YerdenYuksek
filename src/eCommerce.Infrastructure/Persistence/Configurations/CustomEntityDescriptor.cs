namespace eCommerce.Infrastructure.Persistence.Configurations;

public class CustomEntityDescriptor
{
    public CustomEntityDescriptor()
    {
        Fields = new List<CustomEntityFieldDescriptor>();
    }

    public string EntityName { get; set; }

    public string SchemaName { get; set; }

    public ICollection<CustomEntityFieldDescriptor> Fields { get; set; }
}