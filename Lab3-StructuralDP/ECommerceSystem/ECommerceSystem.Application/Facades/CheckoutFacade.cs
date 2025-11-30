using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Notifications;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Shipping;
using ECommerceSystem.Domain.Validation;
using ECommerceSystem.Application.Services;
using ECommerceSystem.Application.Processors;

namespace ECommerceSystem.Application.Facades;

public class CheckoutFacade : ICheckoutFacade
{
    private readonly IInventorySystem _inventorySystem;
    private readonly IShippingService _shippingService;
    private readonly INotificationService _notificationService;
    private readonly IOrderValidator _validatorChain;

    public CheckoutFacade(
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

    public bool PlaceOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender)
    {
        OrderProcessor processor = order.OrderType switch
        {
            OrderType.Express => new ExpressOrderProcessor(_inventorySystem, _shippingService, _notificationService, _validatorChain),
            OrderType.PreOrder => new PreOrderProcessor(_inventorySystem, _shippingService, _notificationService, _validatorChain),
            _ => new StandardOrderProcessor(_inventorySystem, _shippingService, _notificationService, _validatorChain)
        };

        return processor.ProcessOrder(order, paymentGateway, notificationSender);
    }
}