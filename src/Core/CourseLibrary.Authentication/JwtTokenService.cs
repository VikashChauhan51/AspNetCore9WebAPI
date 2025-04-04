using CourseLibrary.Authentication.Configurations;
using CourseLibrary.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseLibrary.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly AuthenticationConfiguration _authenticationConfiguration;
    public JwtTokenService(AuthenticationConfiguration authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration;
    }
    public string? GetToken(User user)
    {
        if (user is null)
        {
            return null;
        }
        var securityKey = new SymmetricSecurityKey(
          Encoding.ASCII.GetBytes(_authenticationConfiguration.SecretForKey));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim("given_name", user.FirstName),
            new Claim("family_name", user.LastName),
            new Claim("email", user.Email)
        };

        var jwtSecurityToken = new JwtSecurityToken(
            _authenticationConfiguration.Issuer,
            _authenticationConfiguration.Audience,
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(5),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler()
           .WriteToken(jwtSecurityToken);
        return tokenToReturn;
    }
}
