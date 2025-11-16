namespace ECommerceSystem.Domain.Payment;

public class PayPalGateway : IPaymentGateway
{
    public string GetName() => "PayPal";

    public bool ProcessPayment(decimal amount, string orderId)
    {
        Console.WriteLine($"Processing payment via PayPal: ${amount} for order {orderId}");
        return true;
    }
}