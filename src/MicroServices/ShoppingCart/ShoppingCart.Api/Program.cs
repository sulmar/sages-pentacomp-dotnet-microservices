using HealthChecks.UI.Client;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ShoppingCart.Api.Mappers;
using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("shoppingcartdb"));

builder.Services.AddTransient<ICartItemRepository, RedisCartItemRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => connection);

builder.Services.AddSingleton<Context>();

// dotnet add package AspNetCore.HealthChecks.Redis
builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("shoppingcartdb"), name: "shoppingcartdb");


#if DEBUG

// dotnet add package OpenTelemetry.Extensions.Hosting
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});


builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation() // dotnet add package OpenTelemetry.Instrumentation.AspNetCore
            .AddHttpClientInstrumentation() // dotnet add package OpenTelemetry.Instrumentation.Http
            .AddRuntimeInstrumentation()   // dotnet add package OpenTelemetry.Instrumentation.Runtime            
            //.AddConsoleExporter();         // dotnet add package OpenTelemetry.Exporter.Console
            .AddOtlpExporter();             // dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
        // OTLP 
    })
    .WithTracing(tracing =>
    {
        tracing
           .AddAspNetCoreInstrumentation() // dotnet add package OpenTelemetry.Instrumentation.AspNetCore
           .AddHttpClientInstrumentation() // dotnet add package OpenTelemetry.Instrumentation.Http
           .AddRedisInstrumentation(connection)
                                           //.AddConsoleExporter();
           .AddOtlpExporter();             // dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
    })
    .ConfigureResource(resource => resource.AddService(serviceName: "shoppingcart-api"));

#endif

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
