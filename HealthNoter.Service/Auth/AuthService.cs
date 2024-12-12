using HealthNoter.Core.Entities;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;
using HealthNoter.Service.Entity.Base;
using HealthNoter.Service.Hash;
using HealthNoter.Service.JWT;

namespace HealthNoter.Service.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IHashService _hashService;

    public AuthService(IUserService userService, IUserRepository userRepository, IJwtService jwtService, IHashService hashService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _hashService = hashService;
    }
    
    public async Task<string> Login(string username, string password, CancellationToken ct)
    {
        var user = await _userRepository.GetByUsernameAndPassword(username, _hashService.ComputeHash(password), ct);
        if (user == null) throw new KeyNotFoundException();

        var token = _jwtService.GenerateToken(user.Id);
        return token;
    }

    public async Task<bool> Register(string username, string password, CancellationToken ct)
    {
        var user = await _userRepository.GetByUsernameAndPassword(username, _hashService.ComputeHash(password), ct);
        if (user != null) throw new ArgumentException();

        return await _userService.Add(username, _hashService.ComputeHash(password), ct);
    }
}