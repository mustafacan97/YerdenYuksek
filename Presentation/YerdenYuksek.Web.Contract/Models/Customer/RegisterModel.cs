namespace YerdenYuksek.Web.Contract.Models.Customer;

public record RegisterModel : BaseModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}