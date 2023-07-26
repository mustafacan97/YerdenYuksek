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

    #endregion

    #region Constructure and Destructure

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region Public Methods

    public string GenerateJwtToken(string email, IEnumerable<string> roles)
    {
        var jwtKey = _configuration["Jwt:Key"];
        if (jwtKey is null)
        {
            return string.Empty;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email)
        };

        foreach (var customerRole in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, customerRole));
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    #endregion
}
