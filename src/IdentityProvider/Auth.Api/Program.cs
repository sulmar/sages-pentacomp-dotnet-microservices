using Auth.Api.Abstractions;
using Auth.Api.Infrastructure;
using Auth.Api.Model;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();
builder.Services.AddSingleton<IIdentityUserRepository, FakeIdentityUserRepository>();
builder.Services.AddSingleton<IPasswordHasher<Auth.Api.Model.IdentityUser>, PasswordHasher<Auth.Api.Model.IdentityUser>>();

var app = builder.Build();

app.MapGet("/", () => "Hello Auth.Api!");

app.MapPost("api/login", async (LoginModel model, IAuthService authService, ITokenService tokenService) =>
{
    var result = await authService.AuthorizeAsync(model.Username, model.Password);

    if (result.IsAuthenticated)
    {
        var accessToken = tokenService.CreateAccessToken(result.Identity);

        return Results.Ok(accessToken);
    }

    return Results.Unauthorized();

});


app.Run();
