namespace eCommerce.Core.Services.Security;

public interface IJwtService
{
    string GenerateJwtToken(string email, IEnumerable<string> roles);
}