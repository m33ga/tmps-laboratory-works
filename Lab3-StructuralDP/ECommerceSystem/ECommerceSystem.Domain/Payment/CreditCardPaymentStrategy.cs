namespace ECommerceSystem.Domain.Payment;

public class CreditCardPaymentStrategy : IPaymentGateway
{
    public string GetName() => "Credit Card";

    public bool ProcessPayment(decimal amount, string orderId)
    {
        Console.WriteLine($"Processing payment via Credit Card: ${amount} for order {orderId}");
        Console.WriteLine("Validating card number...");
        Console.WriteLine("Checking CVV...");
        Console.WriteLine("Verifying expiration date...");
        Console.WriteLine("Authorizing transaction...");
        Console.WriteLine("Credit card payment approved");
        return true;
    }
}