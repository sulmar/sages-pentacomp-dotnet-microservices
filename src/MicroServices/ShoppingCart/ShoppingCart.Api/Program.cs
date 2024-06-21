using HealthChecks.UI.Client;
using ShoppingCart.Api.Mappers;
using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ICartItemRepository, RedisCartItemRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("shoppingcartdb")));

builder.Services.AddSingleton<Context>();

// dotnet add package AspNetCore.HealthChecks.Redis
builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("shoppingcartdb"), name: "shoppingcartdb");

var app = builder.Build();

app.MapGet("/", () => "Hello Shopping Cart Api!");

app.MapPost("api/cart", (Product product, ICartItemRepository repository) =>
{
    var cartItem = ProductToCartItemMapper.Map(product);

    repository.Add(cartItem);
});


app.MapHealthChecks("/api/cart/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    // dotnet add package AspNetCore.HealthChecks.UI.Client
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
