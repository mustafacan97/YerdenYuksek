namespace eCommerce.Application.Services.Public.Security;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string email);
}