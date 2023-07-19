namespace eCommerce.Web.ViewModels;

public record RegisterRequestModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}