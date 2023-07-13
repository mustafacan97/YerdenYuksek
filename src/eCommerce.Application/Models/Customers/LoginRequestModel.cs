namespace eCommerce.Application.Models.Customers;

public record LoginRequestModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}
