using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Customers;

public class CustomerPassword
{
    #region Constructure and Destructure

    public CustomerPassword()
    {
        PasswordFormat = PasswordFormat.Clear;
    }

    #endregion

    #region Public Properties

    public Guid CustomerId { get; set; }

    public string Password { get; set; }

    public int PasswordFormatId { get; set; }

    public string PasswordSalt { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public PasswordFormat PasswordFormat
    {
        get => (PasswordFormat)PasswordFormatId;
        set => PasswordFormatId = (int)value;
    }

    public Customer Customer { get; set; }

    #endregion
}
