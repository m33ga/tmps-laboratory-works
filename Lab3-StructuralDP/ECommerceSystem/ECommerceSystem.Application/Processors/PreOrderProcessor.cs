using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Shipping;
using ECommerceSystem.Domain.Validation;
using ECommerceSystem.Application.Services;

namespace ECommerceSystem.Application.Processors;

public class PreOrderProcessor : OrderProcessor
{
    public PreOrderProcessor(
        IInventorySystem inventorySystem,
        IShippingService shippingService,
        INotificationService notificationService,
        IOrderValidator validatorChain)
        : base(inventorySystem, shippingService, notificationService, validatorChain)
    {
    }

    protected override bool ProcessPayment(Order order, IPaymentGateway paymentGateway)
    {
        Console.WriteLine("Processing PRE-ORDER payment");
        var totalAmount = order.GetTotal();
        var paymentSuccess = paymentGateway.ProcessPayment(totalAmount, order.OrderId);

        if (paymentSuccess)
        {
            order.Status = OrderStatus.PaymentProcessed;
        }

        return paymentSuccess;
    }

    protected override void UpdateInventory(Order order)
    {
        Console.WriteLine("PRE-ORDER: Inventory will be reserved upon product release");
    }

    protected override void ArrangeShipping(Order order)
    {
        Console.WriteLine("PRE-ORDER: Shipping will be arranged upon product release");
        order.Status = OrderStatus.Pending;
    }
}