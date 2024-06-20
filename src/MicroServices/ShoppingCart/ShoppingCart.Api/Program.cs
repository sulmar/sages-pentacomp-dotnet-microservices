using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ICartItemRepository, RedisCartItemRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = { "localhost:6379" }
}));

builder.Services.AddSingleton<Context>();

var app = builder.Build();

app.MapGet("/", () => "Hello Shopping Cart Api!");

app.MapPost("api/cart", (Product product, ICartItemRepository repository) =>
{
    var cartItem = new CartItem { Product = product };

    repository.Add(cartItem);
});

app.Run();
