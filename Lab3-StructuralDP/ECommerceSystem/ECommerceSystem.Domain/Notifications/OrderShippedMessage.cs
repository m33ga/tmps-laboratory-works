namespace ECommerceSystem.Domain.Notifications;

public class OrderShippedMessage : INotificationMessage
{
    private readonly string _orderId;
    private readonly string _trackingNumber;

    public OrderShippedMessage(string orderId, string trackingNumber)
    {
        _orderId = orderId;
        _trackingNumber = trackingNumber;
    }

    public void Send(INotificationSender sender)
    {
        var message = $"Order {_orderId} shipped - Tracking: {_trackingNumber}";
        sender.Send(message);
    }
}