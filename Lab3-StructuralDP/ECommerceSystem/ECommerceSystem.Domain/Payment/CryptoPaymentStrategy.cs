namespace ECommerceSystem.Domain.Payment;

public class CryptoPaymentStrategy : IPaymentGateway
{
    public string GetName() => "Cryptocurrency";

    public bool ProcessPayment(decimal amount, string orderId)
    {
        Console.WriteLine($"Processing payment via Cryptocurrency: ${amount} for order {orderId}");
        Console.WriteLine("Generating wallet address...");
        Console.WriteLine("Waiting for blockchain confirmation...");
        Console.WriteLine("Transaction confirmed on blockchain");
        Console.WriteLine("Cryptocurrency payment successful");
        return true;
    }
}