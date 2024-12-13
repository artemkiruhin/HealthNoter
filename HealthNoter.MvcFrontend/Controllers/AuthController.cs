using HealthNoter.MvcFrontend.Contracts.ViewModels;
using HealthNoter.Service.Auth;
using HealthNoter.Service.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthNoter.MvcFrontend.Controllers;

//[Route("[controller]/")]
[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;

    public AuthController(IAuthService authService, ILogger<AuthController> logger, IConfiguration configuration, IJwtService jwtService)
    {
        _authService = authService;
        _logger = logger;
        _configuration = configuration;
        _jwtService = jwtService;
    }
    
    //[HttpGet("login")]
    public IActionResult Login()
    {
        var token = Request.Cookies["jwt"];
        if (token != null)
        {
            var claimPrincipals = _jwtService.ValidateJwtToken(token);
            if (claimPrincipals != null) return RedirectToAction("Index", "Home");
        }
        
        return View(model: new LoginViewModel());
    }
    
    //[HttpGet("registration")]
    public IActionResult Registration()
    {
        return View(model: new RegisterViewModel());
    }
    
    //[HttpPost("on-login")]
    public async Task<IActionResult> OnLogin(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Login", model);
        }

        try
        {
            var token = await _authService.Login(model.Username, model.Password, HttpContext.RequestAborted);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,              
                SameSite = SameSiteMode.Lax,  
                Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["jwt:expires"] ?? throw new NullReferenceException()))
            });

            _logger.LogInformation($"Пользователь {model.Username} авторизовался | {DateTime.Now}");
            
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _logger.LogError($"Ошибка авторизации пользователя {model.Username} | {DateTime.Now}\nПояснение {e.Message}");
            ModelState.AddModelError(string.Empty, "Ошибка авторизации. Пожалуйста, попробуйте снова.");
            return RedirectToAction("Login", model);
        }
    }
    
    //[HttpPost("on-register")]
    public async Task<IActionResult> OnRegister(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Registration", model);
        }

        try
        {
            var result = await _authService.Register(model.Username, model.Password, HttpContext.RequestAborted);

            if (result)
            {
                _logger.LogInformation($"Пользователь {model.Username} зарегистрировался | {DateTime.Now}");
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, "Ошибка регистрации. Пожалуйста, попробуйте снова.");
            return RedirectToAction("Registration", model);
        }
        catch (Exception e)
        {
            _logger.LogError($"Ошибка регистрации | {DateTime.Now}\nПояснение:{e.Message}\n Подробности: {e}");
            ModelState.AddModelError(string.Empty, "Ошибка регистрации. Пожалуйста, попробуйте снова.");
            return RedirectToAction("Registration", model);
        }
    }
}
