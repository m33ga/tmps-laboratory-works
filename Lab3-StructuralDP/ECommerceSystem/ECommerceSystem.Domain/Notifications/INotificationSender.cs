namespace ECommerceSystem.Domain.Notifications;

public interface INotificationSender
{
    string GetChannel();
    void Send(string message);
}