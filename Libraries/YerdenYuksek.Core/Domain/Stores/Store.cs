using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Stores;

public class Store : BaseEntity, ISoftDeletedEntity
{
    #region Constructure and Destructure

    public Store()
    {
        AllRegisteredCustomers = new HashSet<Customer>();
    }

    #endregion

    #region Public Properties

    public string Name { get; set; }

    public string DefaultMetaKeywords { get; set; }

    public string DefaultMetaDescription { get; set; }

    public string DefaultTitle { get; set; }

    public string HomepageTitle { get; set; }

    public string HomepageDescription { get; set; }

    public string Url { get; set; }

    public bool SslEnabled { get; set; }

    public string Hosts { get; set; }

    public Guid DefaultLanguageId { get; set; }

    public int DisplayOrder { get; set; }

    public string CompanyName { get; set; }

    public string CompanyAddress { get; set; }

    public string CompanyPhoneNumber { get; set; }

    public string CompanyVat { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public ICollection<Customer> AllRegisteredCustomers { get; set; }

    #endregion
}
