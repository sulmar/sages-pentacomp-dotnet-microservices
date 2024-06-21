using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities;
using Catalog.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IProductRepository, FakeProductRepository>();
builder.Services.AddSingleton<Context>(_ =>
{
    var products = new List<Product>
    {
        new Product() { Id = 1, Name = "Product 1", Price = 1.99m, DiscountedPrice = 1.99m },
        new Product() { Id = 2, Name = "Product 2", Price = 10.99m, DiscountedPrice = 9.99m },
        new Product() { Id = 3, Name = "Product 3", Price = 100.99m, DiscountedPrice = 100.99m },
        new Product() { Id = 4, Name = "Product 4", Price = 1.99m, DiscountedPrice = 1.99m },
        new Product() { Id = 5, Name = "Product 5", Price = 50.99m, DiscountedPrice = 40.99m },
    };

    return new Context {  Products = products.ToDictionary(p=>p.Id) };    
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
}));


builder.Services.AddHealthChecks()
    .AddCheck("ping", () => HealthCheckResult.Healthy())
    .AddCheck("random", () =>
    {
        if (DateTime.Now.Minute % 2 == 0)
        {
            return HealthCheckResult.Healthy();
        }
        else
            return HealthCheckResult.Unhealthy();
    });

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseCors();
//}

app.MapGet("/", () => "Hello Catalog Api!");

app.MapGet("api/products/ping", () => "pong");


app.MapGet("api/products", async (IProductRepository repository, HttpContext context) =>
{
    if (context.User.Identity.IsAuthenticated)
    {

    }

    return await repository.GetAllAsync();
});

app.MapHealthChecks("/api/products/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    // dotnet add package AspNetCore.HealthChecks.UI.Client
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
