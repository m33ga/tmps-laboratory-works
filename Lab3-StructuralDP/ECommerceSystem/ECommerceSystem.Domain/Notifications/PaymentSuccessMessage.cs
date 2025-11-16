namespace ECommerceSystem.Domain.Notifications;

public class PaymentSuccessMessage : INotificationMessage
{
    private readonly string _orderId;
    private readonly decimal _amount;
    private readonly string _paymentMethod;

    public PaymentSuccessMessage(string orderId, decimal amount, string paymentMethod)
    {
        _orderId = orderId;
        _amount = amount;
        _paymentMethod = paymentMethod;
    }

    public void Send(INotificationSender sender)
    {
        var message = $"Payment of ${_amount} via {_paymentMethod} successful for order {_orderId}";
        sender.Send(message);
    }
}