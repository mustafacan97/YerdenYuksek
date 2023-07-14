namespace eCommerce.Core.Domain.Customers;

public class CustomerSecurity
{
    #region Constructure and Destructure

    public CustomerSecurity()
    {
    }

    #endregion

    #region Public Properties

    public Guid CustomerId { get; set; }

    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public string? LastIpAddress { get; set; }

    public bool RequireReLogin { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? CannotLoginUntilDateUtc { get; set; }

    #endregion
}
