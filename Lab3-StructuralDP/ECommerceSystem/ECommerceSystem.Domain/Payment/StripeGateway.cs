namespace ECommerceSystem.Domain.Payment;

public class StripeGateway : IPaymentGateway
{
    public string GetName() => "Stripe";

    public bool ProcessPayment(decimal amount, string orderId)
    {
        Console.WriteLine($"Processing payment via Stripe: ${amount} for order {orderId}");
        return true;
    }
}