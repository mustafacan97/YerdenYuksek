namespace YerdenYuksek.Application.Models.Customers;

public record RegisterRequestModel : BaseModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}