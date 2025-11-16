using ECommerceSystem.Domain.Products;

namespace ECommerceSystem.Domain.Orders;

public class OrderItem
{
    public IProductComponent Product { get; }
    public int Quantity { get; }
    public decimal Price { get; }

    public OrderItem(IProductComponent product, int quantity)
    {
        Product = product;
        Quantity = quantity;
        Price = product.GetPrice() * quantity;
    }
}