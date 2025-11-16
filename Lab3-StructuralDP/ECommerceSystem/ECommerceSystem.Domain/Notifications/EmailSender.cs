namespace ECommerceSystem.Domain.Notifications;

public class EmailSender : INotificationSender
{
    public string GetChannel() => "Email";

    public void Send(string message)
    {
        Console.WriteLine($"[Email] {message}");
    }
}