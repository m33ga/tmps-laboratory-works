using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Notifications;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Shipping;
using ECommerceSystem.Domain.Products;
using ECommerceSystem.Application.Services;

namespace ECommerceSystem.Application.Facades;

public class CheckoutFacade : ICheckoutFacade
{
    private readonly IInventorySystem _inventorySystem;
    private readonly IShippingService _shippingService;
    private readonly INotificationService _notificationService;

    public CheckoutFacade(
        IInventorySystem inventorySystem,
        IShippingService shippingService,
        INotificationService notificationService)
    {
        _inventorySystem = inventorySystem;
        _shippingService = shippingService;
        _notificationService = notificationService;
    }

    public bool PlaceOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender)
    {
        Console.WriteLine($"\nProcessing order {order.OrderId} for {order.CustomerName}");

        if (!CheckInventoryAvailability(order))
        {
            Console.WriteLine("Order failed: Insufficient inventory");
            return false;
        }

        var totalAmount = order.GetTotal();
        var paymentSuccess = paymentGateway.ProcessPayment(totalAmount, order.OrderId);

        if (!paymentSuccess)
        {
            Console.WriteLine("Order failed: Payment processing failed");
            return false;
        }

        order.Status = OrderStatus.PaymentProcessed;

        ReserveInventory(order);

        var paymentMessage = new PaymentSuccessMessage(order.OrderId, totalAmount, paymentGateway.GetName());
        _notificationService.SendNotification(paymentMessage, notificationSender);

        var trackingNumber = _shippingService.ArrangeShipping(order.OrderId, order.CustomerName);
        order.TrackingNumber = trackingNumber;
        order.Status = OrderStatus.Shipped;

        var shippedMessage = new OrderShippedMessage(order.OrderId, trackingNumber);
        _notificationService.SendNotification(shippedMessage, notificationSender);

        Console.WriteLine($"Order {order.OrderId} completed successfully\n");
        return true;
    }

    private bool CheckInventoryAvailability(Order order)
    {
        foreach (var item in order.Items)
        {
            var productIds = GetProductIds(item.Product);
            foreach (var productId in productIds)
            {
                if (!_inventorySystem.CheckAvailability(productId, item.Quantity))
                {
                    Console.WriteLine($"Product {productId} not available in requested quantity");
                    return false;
                }
            }
        }
        return true;
    }

    private void ReserveInventory(Order order)
    {
        foreach (var item in order.Items)
        {
            var productIds = GetProductIds(item.Product);
            foreach (var productId in productIds)
            {
                _inventorySystem.ReserveStock(productId, item.Quantity);
            }
        }
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