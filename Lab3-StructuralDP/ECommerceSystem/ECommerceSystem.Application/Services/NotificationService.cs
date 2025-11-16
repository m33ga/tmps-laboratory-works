using ECommerceSystem.Domain.Notifications;

namespace ECommerceSystem.Application.Services;

public class NotificationService : INotificationService
{
    public void SendNotification(INotificationMessage message, INotificationSender sender)
    {
        message.Send(sender);
    }
}