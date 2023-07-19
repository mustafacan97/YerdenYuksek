namespace eCommerce.Web.ViewModels;

public record LoginRequestModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}