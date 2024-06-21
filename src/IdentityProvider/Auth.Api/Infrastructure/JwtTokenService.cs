using Auth.Api.Abstractions;
using Auth.Api.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Infrastructure;

// dotnet add package System.IdentityModel.Tokens.Jwt
// nowsza: Microsoft.IdentityModel.JsonWebTokens
public class JwtTokenService : ITokenService
{
    public string CreateAccessToken(IdentityUser identityUser)
    {
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim("fn", identityUser.FirstName));
        identity.AddClaim(new Claim("ln", identityUser.LastName));
        identity.AddClaim(new Claim("email", identityUser.Email));
        identity.AddClaim(new Claim(ClaimTypes.Role, "developer"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "trainer"));

        var secretKey = "your-256-bit-secret-key-your-256-bit-secret-key-your-256-bit-secret-key-your-256-bit-secret-key";

        var key = Encoding.ASCII.GetBytes(secretKey);

        var credentials = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256Signature);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = signingCredentials,
            Issuer = "https://sages.pl",
            Audience = "https://domain.com"
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var token = tokenHandler.WriteToken(securityToken);

        return token;

    }
}