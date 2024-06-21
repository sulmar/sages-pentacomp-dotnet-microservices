using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = "your-256-bit-secret-key-your-256-bit-secret-key-your-256-bit-secret-key-your-256-bit-secret-key";
    var key = Encoding.ASCII.GetBytes(secretKey);

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = true,
        ValidIssuer = "https://sages.pl",

        ValidateAudience = true,
        ValidAudience = "https://domain.com"
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["access-token"];
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization(options =>
{
    // domyslna regu³a
    //options.AddPolicy("default", policy =>
    //{
    //    policy.RequireAuthenticatedUser();
    //});

    options.AddPolicy("vip-policy", policy =>
    {
        policy
            .RequireAuthenticatedUser()
            .RequireRole("trainer");
    });
});

// dotnet add package Yarp.ReverseProxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.MapGet("api/secret", (HttpContext context) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        return Results.Ok("your secret");
    }
    else
        return Results.Unauthorized();
}).RequireAuthorization("vip-policy");

// [Authorize(Policies="vip-policy")]

app.Run();
