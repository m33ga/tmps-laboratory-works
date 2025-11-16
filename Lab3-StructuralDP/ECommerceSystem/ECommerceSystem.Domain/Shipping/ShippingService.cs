namespace ECommerceSystem.Domain.Shipping;

public class ShippingService : IShippingService
{
    public string ArrangeShipping(string orderId, string customerName)
    {
        var trackingNumber = $"TRCKNGNMBR{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        Console.WriteLine($"Shipping arranged for order {orderId} - Tracking: {trackingNumber}");
        return trackingNumber;
    }
}