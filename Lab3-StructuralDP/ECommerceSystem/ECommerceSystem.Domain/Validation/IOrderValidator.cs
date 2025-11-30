using ECommerceSystem.Domain.Orders;

namespace ECommerceSystem.Domain.Validation;

public interface IOrderValidator
{
    IOrderValidator SetNext(IOrderValidator next);
    bool Validate(Order order);
}