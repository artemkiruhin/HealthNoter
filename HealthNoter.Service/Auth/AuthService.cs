using HealthNoter.Core.Entities;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;
using HealthNoter.Service.Entity.Base;
using HealthNoter.Service.JWT;

namespace HealthNoter.Service.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserService userService, IUserRepository userRepository, IJwtService jwtService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    
    public async Task<string> Login(string username, string password, CancellationToken ct)
    {
        var user = await _userRepository.GetByUsernameAndPassword(username, password, ct);
        if (user == null) throw new KeyNotFoundException();

        var token = _jwtService.GenerateToken(user.Id);
        return token;
    }

    public async Task<bool> Register(string username, string password, CancellationToken ct)
    {
        var user = await _userRepository.GetByUsernameAndPassword(username, password, ct);
        if (user != null) throw new ArgumentException();

        return await _userService.Add(username, password, ct);
    }
}