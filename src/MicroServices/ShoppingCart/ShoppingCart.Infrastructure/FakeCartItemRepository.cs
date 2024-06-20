using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure;

public class FakeCartItemRepository(Context _context) : ICartItemRepository
{
    public Task Add(CartItem item)
    {
        if (_context.Items.Contains(item))
        {
            _context.Items.TryGetValue(item, out var _item);
            _item.Quantity++;
        }
        else
            _context.Items.Add(item);   

        return Task.CompletedTask;
    }

    public Task Clear()
    {
        _context.Items.Clear();

        return Task.CompletedTask;
    }
}

public class Context
{
    public HashSet<CartItem> Items { get; set; } = new HashSet<CartItem>();
}
