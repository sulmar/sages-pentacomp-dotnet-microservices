using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Domain.Abstractions;

public interface ICartRepository
{
    Task Add(Cart cart);
}

public interface ICartItemRepository
{
    Task Add(CartItem item);
}
