using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Notifications;

namespace ECommerceSystem.Application.Facades;

public interface ICheckoutFacade
{
    bool PlaceOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender);
}