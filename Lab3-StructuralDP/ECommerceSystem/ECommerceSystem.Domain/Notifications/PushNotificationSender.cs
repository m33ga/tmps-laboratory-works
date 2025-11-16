namespace ECommerceSystem.Domain.Notifications;

public class PushNotificationSender : INotificationSender
{
    public string GetChannel() => "Push";

    public void Send(string message)
    {
        Console.WriteLine($"[Push] {message}");
    }
}