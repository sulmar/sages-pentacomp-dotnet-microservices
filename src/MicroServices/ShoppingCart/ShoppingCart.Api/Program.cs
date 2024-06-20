using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICartItemRepository, FakeCartItemRepository>();
builder.Services.AddSingleton<Context>();

var app = builder.Build();

app.MapGet("/", () => "Hello Shopping Cart Api!");

app.MapPost("api/cart", (Product product, ICartItemRepository repository) =>
{    
    var cartItem = new CartItem { Product = product };

    repository.Add(cartItem);
});

app.Run();
