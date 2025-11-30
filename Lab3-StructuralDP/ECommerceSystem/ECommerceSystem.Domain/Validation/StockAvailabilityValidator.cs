using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Products;

namespace ECommerceSystem.Domain.Validation;

public class StockAvailabilityValidator : OrderValidator
{
    private readonly IInventorySystem _inventorySystem;

    public StockAvailabilityValidator(IInventorySystem inventorySystem)
    {
        _inventorySystem = inventorySystem;
    }

    public override bool Validate(Order order)
    {
        Console.WriteLine("Validating stock availability...");

        foreach (var item in order.Items)
        {
            var productIds = GetProductIds(item.Product);
            foreach (var productId in productIds)
            {
                if (!_inventorySystem.CheckAvailability(productId, item.Quantity))
                {
                    Console.WriteLine($"Validation failed: Product {productId} not available in requested quantity");
                    return false;
                }
            }
        }

        Console.WriteLine("Stock availability validation passed");
        return base.Validate(order);
    }

    private List<string> GetProductIds(IProductComponent component)
    {
        var productIds = new List<string>();

        if (component is Product product)
        {
            productIds.Add(product.Id);
        }
        else if (component is ProductBundle bundle)
        {
            foreach (var child in bundle.GetComponents())
            {
                productIds.AddRange(GetProductIds(child));
            }
        }

        return productIds;
    }
}