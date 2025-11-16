namespace ECommerceSystem.Domain.Orders;

public class Order
{
    private static int _idCounter = 1;

    public string OrderId { get; }
    public string CustomerName { get; }
    public List<OrderItem> Items { get; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; }
    public string? TrackingNumber { get; set; }

    public Order(string customerName)
    {
        OrderId = $"ORD{_idCounter:D3}";
        _idCounter++;
        CustomerName = customerName;
        Items = new List<OrderItem>();
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.Now;
    }

    public void AddItem(OrderItem item)
    {
        Items.Add(item);
    }

    public decimal GetTotal()
    {
        return Items.Sum(i => i.Price);
    }

    public void Display()
    {
        Console.WriteLine($"Order: {OrderId}");
        Console.WriteLine($"Customer: {CustomerName}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine("Items:");
        foreach (var item in Items)
        {
            Console.WriteLine($"  Quantity: {item.Quantity}");
            item.Product.Display(2);
        }
        Console.WriteLine($"Total: ${GetTotal()}");
    }
}