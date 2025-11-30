# Behavioral Design Patterns

## Author: Mihai Gurduza, FAF-233

----

## ECom Order Processing

Extended e-commerce backend with behavioral design patterns. Building on top of the [structural patterns lab](/Lab3-StructuralDP).

### Used Design Patterns:

* **Strategy** - For encapsulating different payment processing algorithms
* **Chain of Responsibility** - For handling order validation through a chain of validators
* **Template Method** - For defining order processing workflow with customizable steps

#### Strategy Pattern

The Strategy pattern encapsulates payment processing algorithms and makes them interchangeable. Each payment gateway implements its own processing logic while maintaining the same interface.

```csharp
public interface IPaymentGateway
{
    string GetName();
    bool ProcessPayment(decimal amount, string orderId);
}
```

Payment strategies with different algorithms:

```csharp
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
```

Using different strategies:

```csharp
var order = new Order("Jared Leto", "456 Oak Ave, Toronto", "Canada", OrderType.Express);
var paymentGateway = new CreditCardPaymentStrategy();
_checkoutFacade.PlaceOrder(order, paymentGateway, notificationSender);
```

#### Chain of Responsibility Pattern

The Chain of Responsibility pattern passes order validation through a chain of validators. Each validator either handles the validation or passes it to the next validator in the chain.

```csharp
public interface IOrderValidator
{
    IOrderValidator SetNext(IOrderValidator next);
    bool Validate(Order order);
}
```

Abstract base class handling chain mechanics:

```csharp
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
```

Concrete validators:

```csharp
public class StockAvailabilityValidator : OrderValidator
{
    private readonly IInventorySystem _inventorySystem;

    public override bool Validate(Order order)
    {
        Console.WriteLine("Validating stock availability...");

        foreach (var item in order.Items)
        {
            var productIds = GetProductIds(item.Product);
            foreach (var productId in productIds)
            {
                if (!_inventorySystem.CheckAvailability(productId, item.Quantity))
                {
                    Console.WriteLine($"Validation failed: Product {productId} not available");
                    return false;
                }
            }
        }

        Console.WriteLine("Stock availability validation passed");
        return base.Validate(order);
    }
}

public class ShippingAddressValidator : OrderValidator
{
    private readonly List<string> _supportedRegions = new List<string> 
    { 
        "USA", "Canada", "UK", "Germany", "France", "Moldova"
    };

    public override bool Validate(Order order)
    {
        Console.WriteLine("Validating shipping address...");

        if (!_supportedRegions.Contains(order.ShippingRegion))
        {
            Console.WriteLine($"Validation failed: We do not ship to {order.ShippingRegion}");
            return false;
        }

        Console.WriteLine("Shipping address validation passed");
        return base.Validate(order);
    }
}
```

Setting up the chain:

```csharp
var stockValidator = new StockAvailabilityValidator(_inventorySystem);
var addressValidator = new ShippingAddressValidator();
stockValidator.SetNext(addressValidator);
```

#### Template Method Pattern

The Template Method pattern defines the skeleton of the order processing algorithm in a base class, allowing subclasses to override specific steps without changing the algorithm structure.

```csharp
public abstract class OrderProcessor
{
    public bool ProcessOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender)
    {
        Console.WriteLine($"\nProcessing {order.OrderType} order {order.OrderId}");

        if (!ValidateOrder(order))
        {
            Console.WriteLine("Order processing failed: Validation error\n");
            return false;
        }

        if (!ProcessPayment(order, paymentGateway))
        {
            Console.WriteLine("Order processing failed: Payment error\n");
            return false;
        }

        UpdateInventory(order);
        ArrangeShipping(order);
        SendNotifications(order, paymentGateway, notificationSender);

        Console.WriteLine($"Order {order.OrderId} completed successfully\n");
        return true;
    }

    protected virtual bool ValidateOrder(Order order) { /* ... */ }
    protected abstract bool ProcessPayment(Order order, IPaymentGateway paymentGateway);
    protected virtual void UpdateInventory(Order order) { /* ... */ }
    protected abstract void ArrangeShipping(Order order);
    protected virtual void SendNotifications(Order order, /* ... */) { /* ... */ }
}
```

Concrete processors customizing specific steps:

```csharp
public class StandardOrderProcessor : OrderProcessor
{
    protected override bool ProcessPayment(Order order, IPaymentGateway paymentGateway)
    {
        var totalAmount = order.GetTotal();
        var paymentSuccess = paymentGateway.ProcessPayment(totalAmount, order.OrderId);

        if (paymentSuccess)
        {
            order.Status = OrderStatus.PaymentProcessed;
        }

        return paymentSuccess;
    }

    protected override void ArrangeShipping(Order order)
    {
        var trackingNumber = _shippingService.ArrangeShipping(order.OrderId, order.CustomerName);
        order.TrackingNumber = trackingNumber;
        order.Status = OrderStatus.Shipped;
    }
}

public class ExpressOrderProcessor : OrderProcessor
{
    protected override void ArrangeShipping(Order order)
    {
        Console.WriteLine("Arranging EXPRESS shipping with priority handling");
        var trackingNumber = _shippingService.ArrangeShipping(order.OrderId, order.CustomerName);
        order.TrackingNumber = trackingNumber;
        order.Status = OrderStatus.Shipped;
        Console.WriteLine("Express delivery scheduled for next business day");
    }
}

public class PreOrderProcessor : OrderProcessor
{
    protected override void UpdateInventory(Order order)
    {
        Console.WriteLine("PRE-ORDER: Inventory will be reserved upon product release");
    }

    protected override void ArrangeShipping(Order order)
    {
        Console.WriteLine("PRE-ORDER: Shipping will be arranged upon product release");
        order.Status = OrderStatus.Pending;
    }
}
```

Using the appropriate processor:

```csharp
OrderProcessor processor = order.OrderType switch
{
    OrderType.Express => new ExpressOrderProcessor(/* ... */),
    OrderType.PreOrder => new PreOrderProcessor(/* ... */),
    _ => new StandardOrderProcessor(/* ... */)
};

return processor.ProcessOrder(order, paymentGateway, notificationSender);
```

### Conclusions

The Strategy pattern made it simple to add new payment methods without modifying existing code. Each payment gateway encapsulates its own processing logic while remaining interchangeable.

The Chain of Responsibility pattern decoupled order validation logic into separate validators. Adding new validation rules is straightforward and doesn't affect existing validators.

The Template Method pattern established a clear order processing workflow while allowing different order types to customize specific steps. The base algorithm remains unchanged while subclasses adapt their behavior.

All three behavioral patterns integrate seamlessly with the existing structural patterns, demonstrating how design patterns work together in real applications.

Demo time:
```
 E-Commerce Order Processing System 

Creating products...

Creating bundles...

 Product Catalog 

Individual Products:
Corsair Mechanical Keyboard - $89,99
Logitech Wireless Mouse - $34,99
4K Webcam - $79,99
Sennheiser Headset - $129,99
LG 27inch Monitor - $299,99
Xiaomi Desk Lamp - $39,99

Product Bundles:
Basic Work From Home Kit (Bundle) - $124,98
  Corsair Mechanical Keyboard - $89,99
  Logitech Wireless Mouse - $34,99

Pro Work From Home Kit (Bundle) - $334,96
  Corsair Mechanical Keyboard - $89,99
  Logitech Wireless Mouse - $34,99
  4K Webcam - $79,99
  Sennheiser Headset - $129,99

Ultra Work From Home Kit (Bundle) - $674,94
  Basic Work From Home Kit (Bundle) - $124,98
    Corsair Mechanical Keyboard - $89,99
    Logitech Wireless Mouse - $34,99
  4K Webcam - $79,99
  Sennheiser Headset - $129,99
  LG 27inch Monitor - $299,99
  Xiaomi Desk Lamp - $39,99

 Initializing Inventory 

Inventory initialized

 Current Inventory 

Corsair Mechanical Keyboard (PRD001): 10 units
Logitech Wireless Mouse (PRD002): 15 units
4K Webcam (PRD003): 8 units
Sennheiser Headset (PRD004): 5 units
LG 27inch Monitor (PRD005): 3 units
Xiaomi Desk Lamp (PRD006): 12 units

 Processing Orders

Order: ORD001
Customer: Kanye West
Status: Pending
Items:
  Quantity: 2
    Corsair Mechanical Keyboard - $89,99
  Quantity: 2
    Logitech Wireless Mouse - $34,99
Total: $249,96


Processing Standard order ORD001 for Kanye West
Validating stock availability...
Stock availability validation passed
Validating shipping address...
Shipping address validation passed
Processing payment via PayPal: $249,96 for order ORD001
Redirecting to PayPal authorization page...
PayPal authorization successful
Updating inventory...
Shipping arranged for order ORD001 - Tracking: TRCKNGNMBR46005D2C
[Email] Payment of $249,96 via PayPal successful for order ORD001
[Email] Order ORD001 shipped - Tracking: TRCKNGNMBR46005D2C
Order ORD001 completed successfully

Order: ORD002
Customer: Jared Leto
Status: Pending
Items:
  Quantity: 1
    Pro Work From Home Kit (Bundle) - $334,96
      Corsair Mechanical Keyboard - $89,99
      Logitech Wireless Mouse - $34,99
      4K Webcam - $79,99
      Sennheiser Headset - $129,99
Total: $334,96


Processing Express order ORD002 for Jared Leto
Validating stock availability...
Stock availability validation passed
Validating shipping address...
Shipping address validation passed
Processing payment via Credit Card: $334,96 for order ORD002
Validating card number...
Checking CVV...
Verifying expiration date...
Authorizing transaction...
Credit card payment approved
Updating inventory...
Arranging EXPRESS shipping with priority handling
Shipping arranged for order ORD002 - Tracking: TRCKNGNMBRCBDBEC81
Express delivery scheduled for next business day
[SMS] Payment of $334,96 via Credit Card successful for order ORD002
[SMS] Order ORD002 shipped - Tracking: TRCKNGNMBRCBDBEC81
Order ORD002 completed successfully

Order: ORD003
Customer: Marcel Bostan
Status: Pending
Items:
  Quantity: 1
    Ultra Work From Home Kit (Bundle) - $674,94
      Basic Work From Home Kit (Bundle) - $124,98
        Corsair Mechanical Keyboard - $89,99
        Logitech Wireless Mouse - $34,99
      4K Webcam - $79,99
      Sennheiser Headset - $129,99
      LG 27inch Monitor - $299,99
      Xiaomi Desk Lamp - $39,99
  Quantity: 2
    Sennheiser Headset - $129,99
Total: $934,92


Processing PreOrder order ORD003 for Marcel Bostan
Validating stock availability...
Stock availability validation passed
Validating shipping address...
Shipping address validation passed
Processing PRE-ORDER payment
Processing payment via PayPal: $934,92 for order ORD003
Redirecting to PayPal authorization page...
PayPal authorization successful
PRE-ORDER: Inventory will be reserved upon product release
PRE-ORDER: Shipping will be arranged upon product release
[Push] Payment of $934,92 via PayPal successful for order ORD003
Order ORD003 completed successfully

Order: ORD004
Customer: Till Lindemann
Status: Pending
Items:
  Quantity: 5
    LG 27inch Monitor - $299,99
Total: $1499,95


Processing Standard order ORD004 for Till Lindemann
Validating stock availability...
Validation failed: Product PRD005 not available in requested quantity
Order processing failed: Validation error

Order: ORD005
Customer: Satoshi Nakamoto
Status: Pending
Items:
  Quantity: 1
    Sennheiser Headset - $129,99
Total: $129,99


Processing Standard order ORD005 for Satoshi Nakamoto
Validating stock availability...
Stock availability validation passed
Validating shipping address...
Validation failed: We do not ship to Japan
Order processing failed: Validation error

 Final Inventory Status 

Corsair Mechanical Keyboard (PRD001): 7 units remaining
Logitech Wireless Mouse (PRD002): 12 units remaining
4K Webcam (PRD003): 7 units remaining
Sennheiser Headset (PRD004): 4 units remaining
LG 27inch Monitor (PRD005): 3 units remaining
Xiaomi Desk Lamp (PRD006): 12 units remaining

Process finished with exit code 0.
```