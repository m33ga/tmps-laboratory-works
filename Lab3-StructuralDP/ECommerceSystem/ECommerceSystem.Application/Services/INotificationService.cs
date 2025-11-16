using ECommerceSystem.Domain.Notifications;

namespace ECommerceSystem.Application.Services;

public interface INotificationService
{
    void SendNotification(INotificationMessage message, INotificationSender sender);
}