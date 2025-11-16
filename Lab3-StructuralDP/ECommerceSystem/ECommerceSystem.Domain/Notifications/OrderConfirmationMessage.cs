namespace ECommerceSystem.Domain.Notifications;

public class OrderConfirmationMessage : INotificationMessage
{
    private readonly string _orderId;
    private readonly string _customerName;

    public OrderConfirmationMessage(string orderId, string customerName)
    {
        _orderId = orderId;
        _customerName = customerName;
    }

    public void Send(INotificationSender sender)
    {
        var message = $"Order {_orderId} confirmed for {_customerName}";
        sender.Send(message);
    }
}