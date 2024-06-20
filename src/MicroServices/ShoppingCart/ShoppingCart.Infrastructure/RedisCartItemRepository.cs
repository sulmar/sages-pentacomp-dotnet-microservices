using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using StackExchange.Redis;

namespace ShoppingCart.Infrastructure;

// dotnet add package StackExchange.Redis
public class RedisCartItemRepository(IConnectionMultiplexer _connectionMultiplexer) : ICartItemRepository
{
    public async Task Add(CartItem item)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var userId = "user123";

        string cartKey = $"cart:{userId}";
        string field = $"product:{item.Product.Id}";

        await db.HashIncrementAsync(cartKey, field, item.Quantity);        
    }

    public async Task Clear()
    {
        var db = _connectionMultiplexer.GetDatabase();
        var userId = "user123";

        string cartKey = $"cart:{userId}";

        await db.KeyDeleteAsync(cartKey);
    }
}