namespace YerdenYuksek.Application.Services.Public.Customers;

public partial interface ICustomerService
{
    Task<bool> IsRegisteredAsync(string email);
}