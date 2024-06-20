

var builder = WebApplication.CreateBuilder(args);

// dotnet add package AspNetCore.HealthCheck.UI
// dotnet add package AspNetCore.HealthCheck.UI.Client
// dotnet add package AspNetCore.HealthCheck.UI.SqLite.Storage

builder.Services
    .AddHealthChecksUI(options =>
    {
        options.AddHealthCheckEndpoint("Catalog Api", "https://localhost:7260/api/products/hc");
        options.AddHealthCheckEndpoint("Shopping Cart Api", "https://localhost:7289/api/cart/hc");
    })
    .AddSqliteStorage(builder.Configuration.GetConnectionString("HealthChecks"));
    

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");

app.MapHealthChecksUI(options => options.UIPath = "/monitor");

app.Run();
