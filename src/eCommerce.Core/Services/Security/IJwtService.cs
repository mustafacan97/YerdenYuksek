namespace eCommerce.Core.Services.Security;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string email);
}