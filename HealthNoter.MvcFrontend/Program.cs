using System.Text;
using HealthNoter.DataAccess.Postgresql;
using HealthNoter.DataAccess.Postgresql.Repositories;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;
using HealthNoter.DataAccess.Postgresql.UoW;
using HealthNoter.MvcFrontend.Controllers;
using HealthNoter.Service.Auth;
using HealthNoter.Service.Entity;
using HealthNoter.Service.Entity.Base;
using HealthNoter.Service.Hash;
using HealthNoter.Service.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = BearerTokenDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = BearerTokenDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["jwt:issuer"],
            ValidAudience = configuration["jwt:audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                context.Response.Redirect("/account/login");
                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                context.Response.Redirect("/account/login");
                return Task.CompletedTask;
            }
        };
    });



builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"))
        .UseLazyLoadingProxies()
        .EnableSensitiveDataLogging(false)
        .EnableDetailedErrors(false);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPressureNoteRepository, PressureNoteRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPressureNoteService, PressureNoteService>();
builder.Services.AddScoped<IHashService, Sha256HashService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();



builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();