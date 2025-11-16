namespace ECommerceSystem.Domain.Orders;

public enum OrderStatus
{
    Pending,
    PaymentProcessed,
    Shipped,
    Delivered,
    Cancelled
}