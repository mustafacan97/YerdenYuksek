namespace YerdenYuksek.Core.Domain.Customers;

public class CustomerPassword
{
    #region Constructure and Destructure

    public CustomerPassword()
    {
        PasswordFormatId = (int)PasswordFormat.Clear;
    }

    #endregion

    #region Public Properties

    public Guid CustomerId { get; set; }

    public string Password { get; set; }

    public int PasswordFormatId { get; set; }

    public string PasswordSalt { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    #endregion

    #region Public Methods

    public PasswordFormat GetPasswordFormat()
    {
        return (PasswordFormat)PasswordFormatId;
    }

    #endregion
}
