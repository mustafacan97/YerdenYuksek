using eCommerce.Core.Services.Customers;
using eCommerce.Core.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eCommerce.Infrastructure.Services.Secuirty;

internal class JwtService : IJwtService
{
    #region Fields

    private readonly IConfiguration _configuration;

    private readonly ICustomerService _customerService;

    #endregion

    #region Constructure and Destructure

    public JwtService(
        IConfiguration configuration,
        ICustomerService customerService)
    {
        _configuration = configuration;
        _customerService = customerService;
    }

    #endregion

    #region Public Methods

    public async Task<string> GenerateJwtToken(string email)
    {
        var customer = await _customerService.GetCustomerByEmailAsync(email);
        if (customer is null)
        {
            return string.Empty;
        }

        var jwtKey = _configuration["Jwt:Key"];
        if (jwtKey is null)
        {
            return string.Empty;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, customer.Email)
        };

        foreach (var customerRole in customer.CustomerRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, customerRole.Name));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    #endregion
}
