namespace HealthNoter.Service.Auth;

public interface IAuthService
{
    Task<string> Login(string username, string password);
    Task<bool> Register(string username, string password);
}