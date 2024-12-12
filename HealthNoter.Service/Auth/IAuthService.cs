namespace HealthNoter.Service.Auth;

public interface IAuthService
{
    Task<string> Login(string username, string password, CancellationToken ct);
    Task<bool> Register(string username, string password, CancellationToken ct);
}