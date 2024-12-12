using System.Security.Claims;

namespace HealthNoter.Service.JWT;

public interface IJwtService
{
    string GenerateToken(Guid id);
    ClaimsPrincipal ValidateJwtToken(string token);
}