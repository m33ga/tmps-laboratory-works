namespace ECommerceSystem.Domain.Payment;

public interface IPaymentGateway
{
    string GetName();
    bool ProcessPayment(decimal amount, string orderId);
}