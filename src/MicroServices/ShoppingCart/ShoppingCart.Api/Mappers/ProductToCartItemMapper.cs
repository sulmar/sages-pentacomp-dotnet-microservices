using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Api.Mappers;

public class ProductToCartItemMapper
{
    public static CartItem Map(Product product)
    {
        return new CartItem { Product = product };
    }

    // Można zastosowac automatyczne generowanie kodu za pomocą biblioteki mapperly
    // https://github.com/riok/mapperly
}
