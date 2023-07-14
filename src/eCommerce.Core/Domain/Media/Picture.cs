using eCommerce.Core.Domain.Customers;
using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Media;

public class Picture : SoftDeletedEntity
{
    public string MimeType { get; set; }

    public string SeoFilename { get; set; }

    public string AltAttribute { get; set; }

    public string TitleAttribute { get; set; }

    public bool IsNew { get; set; }

    public string VirtualPath { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Customer? Customer { get; set; }
}