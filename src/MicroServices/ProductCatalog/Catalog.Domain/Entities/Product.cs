namespace Catalog.Domain.Entities;

public class Product : EntityBase
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountedPrice { get; set; }
}
