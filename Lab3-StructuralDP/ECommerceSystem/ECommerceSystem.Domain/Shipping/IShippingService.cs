namespace ECommerceSystem.Domain.Shipping;

public interface IShippingService
{
    string ArrangeShipping(string orderId, string customerName);
}