namespace ECommerceSystem.Domain.Notifications;

public interface INotificationMessage
{
    void Send(INotificationSender sender);
}