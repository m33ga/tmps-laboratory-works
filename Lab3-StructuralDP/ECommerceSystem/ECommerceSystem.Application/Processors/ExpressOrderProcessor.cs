using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Shipping;
using ECommerceSystem.Domain.Validation;
using ECommerceSystem.Application.Services;

namespace ECommerceSystem.Application.Processors;

public class ExpressOrderProcessor : OrderProcessor
{
    public ExpressOrderProcessor(
        IInventorySystem inventorySystem,
        IShippingService shippingService,
        INotificationService notificationService,
        IOrderValidator validatorChain)
        : base(inventorySystem, shippingService, notificationService, validatorChain)
    {
    }

    protected override bool ProcessPayment(Order order, IPaymentGateway paymentGateway)
    {
        var totalAmount = order.GetTotal();
        var paymentSuccess = paymentGateway.ProcessPayment(totalAmount, order.OrderId);

        if (paymentSuccess)
        {
            order.Status = OrderStatus.PaymentProcessed;
        }

        return paymentSuccess;
    }

    protected override void ArrangeShipping(Order order)
    {
        Console.WriteLine("Arranging EXPRESS shipping with priority handling");
        var trackingNumber = _shippingService.ArrangeShipping(order.OrderId, order.CustomerName);
        order.TrackingNumber = trackingNumber;
        order.Status = OrderStatus.Shipped;
        Console.WriteLine("Express delivery scheduled for next business day");
    }
}