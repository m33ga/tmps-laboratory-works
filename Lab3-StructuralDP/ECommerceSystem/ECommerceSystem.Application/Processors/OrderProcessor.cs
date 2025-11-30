using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Notifications;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Shipping;
using ECommerceSystem.Domain.Validation;
using ECommerceSystem.Domain.Products;
using ECommerceSystem.Application.Services;

namespace ECommerceSystem.Application.Processors;

public abstract class OrderProcessor
{
    protected readonly IInventorySystem _inventorySystem;
    protected readonly IShippingService _shippingService;
    protected readonly INotificationService _notificationService;
    protected readonly IOrderValidator _validatorChain;

    protected OrderProcessor(
        IInventorySystem inventorySystem,
        IShippingService shippingService,
        INotificationService notificationService,
        IOrderValidator validatorChain)
    {
        _inventorySystem = inventorySystem;
        _shippingService = shippingService;
        _notificationService = notificationService;
        _validatorChain = validatorChain;
    }

    public bool ProcessOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender)
    {
        Console.WriteLine($"\nProcessing {order.OrderType} order {order.OrderId} for {order.CustomerName}");

        if (!ValidateOrder(order))
        {
            Console.WriteLine("Order processing failed: Validation error\n");
            return false;
        }

        if (!ProcessPayment(order, paymentGateway))
        {
            Console.WriteLine("Order processing failed: Payment error\n");
            return false;
        }

        UpdateInventory(order);

        ArrangeShipping(order);

        SendNotifications(order, paymentGateway, notificationSender);

        Console.WriteLine($"Order {order.OrderId} completed successfully\n");
        return true;
    }

    protected virtual bool ValidateOrder(Order order)
    {
        return _validatorChain.Validate(order);
    }

    protected abstract bool ProcessPayment(Order order, IPaymentGateway paymentGateway);

    protected virtual void UpdateInventory(Order order)
    {
        Console.WriteLine("Updating inventory...");
        foreach (var item in order.Items)
        {
            var productIds = GetProductIds(item.Product);
            foreach (var productId in productIds)
            {
                _inventorySystem.ReserveStock(productId, item.Quantity);
            }
        }
    }

    protected abstract void ArrangeShipping(Order order);

    protected virtual void SendNotifications(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender)
    {
        var paymentMessage = new PaymentSuccessMessage(order.OrderId, order.GetTotal(), paymentGateway.GetName());
        _notificationService.SendNotification(paymentMessage, notificationSender);

        if (order.TrackingNumber != null)
        {
            var shippedMessage = new OrderShippedMessage(order.OrderId, order.TrackingNumber);
            _notificationService.SendNotification(shippedMessage, notificationSender);
        }
    }

    protected List<string> GetProductIds(IProductComponent component)
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