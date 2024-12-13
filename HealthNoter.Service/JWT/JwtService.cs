using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HealthNoter.Service.JWT;

public class JwtService : IJwtService
{
    private readonly string _key;
    private readonly string _audience;
    private readonly string _issuer;
    private readonly int _expiresInHours;

    public JwtService(string key, string audience, string issuer, int expiresInHours)
    {
        _key = key;
        _audience = audience;
        _issuer = issuer;
        _expiresInHours = expiresInHours;
    }
    
    public string GenerateToken(Guid id)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: creds,
            issuer: _issuer,
            audience: _audience,
            expires: DateTime.Now.AddHours(_expiresInHours)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))
        };
        
        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        if (validatedToken is JwtSecurityToken jwtToken &&
            jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return principal;
        }

        throw new SecurityTokenException("Алгоритм подписи не поддерживается.");
    }

}