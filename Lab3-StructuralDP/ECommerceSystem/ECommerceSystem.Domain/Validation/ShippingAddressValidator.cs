using ECommerceSystem.Domain.Orders;

namespace ECommerceSystem.Domain.Validation;

public class ShippingAddressValidator : OrderValidator
{
    private readonly List<string> _supportedRegions = new List<string> 
    { 
        "USA", "Canada", "UK", "Germany", "France", "Moldova"
    };

    public override bool Validate(Order order)
    {
        Console.WriteLine("Validating shipping address...");

        if (string.IsNullOrWhiteSpace(order.ShippingAddress))
        {
            Console.WriteLine("Validation failed: Shipping address is required");
            return false;
        }

        var region = order.ShippingRegion;
        if (string.IsNullOrWhiteSpace(region) || !_supportedRegions.Contains(region))
        {
            Console.WriteLine($"Validation failed: We do not ship to {region}");
            return false;
        }

        Console.WriteLine("Shipping address validation passed");
        return base.Validate(order);
    }
}