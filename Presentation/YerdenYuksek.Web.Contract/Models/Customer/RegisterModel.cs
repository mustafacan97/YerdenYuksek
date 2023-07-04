namespace YerdenYuksek.Web.Contract.Models.Customer;

public partial record RegisterModel : BaseModel
{
    public string Email { get; set; }
}