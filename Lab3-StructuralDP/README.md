# Structural Design Patterns


## Author: Mihai Gurduza, FAF-233

----

## ECom Order Processing

Over simplified e-commerce backend. Playing with structural design patterns.

### Used Design Patterns:

* **Composite** - For managing products and product bundles
* **Facade** - For simplifying the checkout process
* **Bridge** - For decoupling notification messages from delivery channels

#### Composite Pattern

The Composite pattern allows individual products and bundles to be treated uniformly through the `IProductComponent` interface.

```csharp
public interface IProductComponent
{
    string GetId();
    string GetName();
    decimal GetPrice();
    void Display(int depth = 0);
}
```

A `Product` is a leaf component:

```csharp
public class Product : IProductComponent
{
    public string Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(string name, decimal price)
    {
        Id = $"PRD{_idCounter:D3}";
        _idCounter++;
        Name = name;
        Price = price;
    }

    public decimal GetPrice() => Price;
}
```

A `ProductBundle` is a composite that can contain products and other bundles:

```csharp
public class ProductBundle : IProductComponent
{
    private readonly List<IProductComponent> _components;

    public void Add(IProductComponent component)
    {
        _components.Add(component);
    }

    public decimal GetPrice()
    {
        return _components.Sum(c => c.GetPrice());
    }
}
```

Creating nested bundles:

```csharp
basicBundle = new ProductBundle("Basic Work From Home Kit");
basicBundle.Add(keyboard);
basicBundle.Add(mouse);

ultraBundle = new ProductBundle("Ultra Work From Home Kit");
ultraBundle.Add(basicBundle);
ultraBundle.Add(webcam);
ultraBundle.Add(monitor);
```

#### Facade Pattern

The Facade pattern simplifies the complex checkout process by providing a single entry point that coordinates multiple subsystems.

```csharp
public interface ICheckoutFacade
{
    bool PlaceOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender);
}
```

The `CheckoutFacade` orchestrates inventory checking, payment processing, shipping, and notifications:

```csharp
public class CheckoutFacade : ICheckoutFacade
{
    private readonly IInventorySystem _inventorySystem;
    private readonly IShippingService _shippingService;
    private readonly INotificationService _notificationService;

    public bool PlaceOrder(Order order, IPaymentGateway paymentGateway, INotificationSender notificationSender)
    {
        if (!CheckInventoryAvailability(order))
        {
            Console.WriteLine("Order failed: Insufficient inventory");
            return false;
        }

        var totalAmount = order.GetTotal();
        var paymentSuccess = paymentGateway.ProcessPayment(totalAmount, order.OrderId);

        if (!paymentSuccess)
        {
            Console.WriteLine("Order failed: Payment processing failed");
            return false;
        }

        order.Status = OrderStatus.PaymentProcessed;
        ReserveInventory(order);

        var paymentMessage = new PaymentSuccessMessage(order.OrderId, totalAmount, paymentGateway.GetName());
        _notificationService.SendNotification(paymentMessage, notificationSender);

        var trackingNumber = _shippingService.ArrangeShipping(order.OrderId, order.CustomerName);
        order.TrackingNumber = trackingNumber;
        order.Status = OrderStatus.Shipped;

        var shippedMessage = new OrderShippedMessage(order.OrderId, trackingNumber);
        _notificationService.SendNotification(shippedMessage, notificationSender);

        return true;
    }
}
```

Using the facade:

```csharp
var order = new Order("Kanye West");
order.AddItem(new OrderItem(keyboard, 2));
order.AddItem(new OrderItem(mouse, 2));

var paymentGateway = new PayPalGateway();
var notificationSender = new EmailSender();

_checkoutFacade.PlaceOrder(order, paymentGateway, notificationSender);
```

#### Bridge Pattern

The Bridge pattern decouples notification messages from the way they are sent, allowing them to vary independently.

Abstraction side (what to send):

```csharp
public interface INotificationMessage
{
    void Send(INotificationSender sender);
}

public class PaymentSuccessMessage : INotificationMessage
{
    private readonly string _orderId;
    private readonly decimal _amount;
    private readonly string _paymentMethod;

    public void Send(INotificationSender sender)
    {
        var message = $"Payment of ${_amount} via {_paymentMethod} successful for order {_orderId}";
        sender.Send(message);
    }
}
```

Implementation side (how to send):

```csharp
public interface INotificationSender
{
    string GetChannel();
    void Send(string message);
}

public class EmailSender : INotificationSender
{
    public string GetChannel() => "Email";

    public void Send(string message)
    {
        Console.WriteLine($"[Email] {message}");
    }
}

public class SmsSender : INotificationSender
{
    public string GetChannel() => "SMS";

    public void Send(string message)
    {
        Console.WriteLine($"[SMS] {message}");
    }
}
```

Using the bridge:

```csharp
var message = new PaymentSuccessMessage(orderId, amount, paymentMethod);
var sender = new EmailSender();
message.Send(sender);
```

### Conclusions

The Composite pattern made it easy to handle individual products and nested bundles with the same code.
The Facade pattern cleaned up the checkout flow by hiding all the logic behind one simple method.
The Bridge pattern gave me the possibility to send any type of notification through any channel without creating a lot of specific classes.

Demo time:
```
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


Processing order ORD001 for Kanye West
Processing payment via PayPal: $249,96 for order ORD001
[Email] Payment of $249,96 via PayPal successful for order ORD001
Shipping arranged for order ORD001 - Tracking: TRCKNGNMBR1B59AF6D
[Email] Order ORD001 shipped - Tracking: TRCKNGNMBR1B59AF6D
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


Processing order ORD002 for Jared Leto
Processing payment via Stripe: $334,96 for order ORD002
[SMS] Payment of $334,96 via Stripe successful for order ORD002
Shipping arranged for order ORD002 - Tracking: TRCKNGNMBR554B14E0
[SMS] Order ORD002 shipped - Tracking: TRCKNGNMBR554B14E0
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


Processing order ORD003 for Marcel Bostan
Processing payment via PayPal: $934,92 for order ORD003
[Push] Payment of $934,92 via PayPal successful for order ORD003
Shipping arranged for order ORD003 - Tracking: TRCKNGNMBR195AD2F3
[Push] Order ORD003 shipped - Tracking: TRCKNGNMBR195AD2F3
Order ORD003 completed successfully

 Final Inventory Status 

Corsair Mechanical Keyboard (PRD001): 6 units remaining
Logitech Wireless Mouse (PRD002): 11 units remaining
4K Webcam (PRD003): 6 units remaining
Sennheiser Headset (PRD004): 1 units remaining
LG 27inch Monitor (PRD005): 2 units remaining
Xiaomi Desk Lamp (PRD006): 11 units remaining

Process finished with exit code 0.
```

