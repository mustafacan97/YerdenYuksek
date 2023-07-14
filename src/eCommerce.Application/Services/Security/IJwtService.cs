namespace eCommerce.Application.Services.Security;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string email);
}