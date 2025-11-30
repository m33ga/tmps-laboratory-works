using ECommerceSystem.Domain.Orders;

namespace ECommerceSystem.Domain.Validation;

public abstract class OrderValidator : IOrderValidator
{
    private IOrderValidator _next;

    public IOrderValidator SetNext(IOrderValidator next)
    {
        _next = next;
        return next;
    }

    public virtual bool Validate(Order order)
    {
        if (_next != null)
        {
            return _next.Validate(order);
        }
        return true;
    }
}