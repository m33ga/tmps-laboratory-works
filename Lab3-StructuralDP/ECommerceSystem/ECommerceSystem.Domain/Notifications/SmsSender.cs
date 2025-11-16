namespace ECommerceSystem.Domain.Notifications;

public class SmsSender : INotificationSender
{
    public string GetChannel() => "SMS";

    public void Send(string message)
    {
        Console.WriteLine($"[SMS] {message}");
    }
}