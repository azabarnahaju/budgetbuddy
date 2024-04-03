using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BudgetBuddy.IntegrationTests.JwtAuthenticationTest;

public class TestJwtToken
{
    public List<Claim> Claims { get; } = new();
    public int ExpiresInMinutes { get; set; } = 30;

    public TestJwtToken WithRole(string roleName)
    {
        Claims.Add(new Claim(ClaimTypes.Role, roleName));
        return this;
    }
    
    public TestJwtToken WithName(string username)
    {
        Claims.Add(new Claim(ClaimTypes.Name, username));
        return this;
    }

    public string Build()
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Expires = DateTime.UtcNow.AddMinutes(ExpiresInMinutes),
            SigningCredentials = JwtTokenProvider.SigningCredentials,
            Issuer = "your_fake_valid_issuer",
            Audience = "your_fake_valid_audience"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}