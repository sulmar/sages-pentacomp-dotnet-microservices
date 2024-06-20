namespace ShoppingCart.Domain.Entities;

public record Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }   
}

public record CartItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; } = 1;    
}

public class Cart
{
    public ICollection<CartItem> Items { get; set; } = [];

    public void Add(CartItem item)
    {
        Items.Add(item);
    }
}