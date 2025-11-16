using ECommerceSystem.Domain.Products;
using ECommerceSystem.Domain.Orders;
using ECommerceSystem.Domain.Payment;
using ECommerceSystem.Domain.Notifications;
using ECommerceSystem.Domain.Inventory;
using ECommerceSystem.Domain.Shipping;
using ECommerceSystem.Application.Services;
using ECommerceSystem.Application.Facades;

namespace ECommerceSystem.Console;

class Program
{
    static void Main(string[] args)
    {
        var demo = new DemoScenarios();
        demo.Run();
    }
}

class DemoScenarios
{
    private readonly IInventorySystem _inventorySystem;
    private readonly ICheckoutFacade _checkoutFacade;

    private readonly List<Product> products;
    private readonly List<ProductBundle> bundles;

    private Product keyboard;
    private Product mouse;
    private Product webcam;
    private Product headset;
    private Product monitor;
    private Product deskLamp;

    private ProductBundle basicBundle;
    private ProductBundle proBundle;
    private ProductBundle ultraBundle;

    public DemoScenarios()
    {
        _inventorySystem = new InventorySystem();
        var shippingService = new ShippingService();
        var notificationService = new NotificationService();
        _checkoutFacade = new CheckoutFacade(_inventorySystem, shippingService, notificationService);

        products = new List<Product>();
        bundles = new List<ProductBundle>();
    }

    public void Run()
    {
        System.Console.WriteLine(" E-Commerce Order Processing System \n");

        CreateProducts();
        CreateBundles();

        DisplayCatalog();

        InitializeInventory();
        DisplayInventory();

        ProcessOrders();

        DisplayFinalInventory();
    }

    private void CreateProducts()
    {
        System.Console.WriteLine("Creating products...\n");

        keyboard = new Product("Corsair Mechanical Keyboard", 89.99m);
        mouse = new Product("Logitech Wireless Mouse", 34.99m);
        webcam = new Product("4K Webcam", 79.99m);
        headset = new Product("Sennheiser Headset", 129.99m);
        monitor = new Product("LG 27inch Monitor", 299.99m);
        deskLamp = new Product("Xiaomi Desk Lamp", 39.99m);

        products.Add(keyboard);
        products.Add(mouse);
        products.Add(webcam);
        products.Add(headset);
        products.Add(monitor);
        products.Add(deskLamp);
    }

    private void CreateBundles()
    {
        System.Console.WriteLine("Creating bundles...\n");

        basicBundle = new ProductBundle("Basic Work From Home Kit");
        basicBundle.Add(keyboard);
        basicBundle.Add(mouse);

        proBundle = new ProductBundle("Pro Work From Home Kit");
        proBundle.Add(keyboard);
        proBundle.Add(mouse);
        proBundle.Add(webcam);
        proBundle.Add(headset);

        ultraBundle = new ProductBundle("Ultra Work From Home Kit");
        ultraBundle.Add(basicBundle);
        ultraBundle.Add(webcam);
        ultraBundle.Add(headset);
        ultraBundle.Add(monitor);
        ultraBundle.Add(deskLamp);

        bundles.Add(basicBundle);
        bundles.Add(proBundle);
        bundles.Add(ultraBundle);
    }

    private void DisplayCatalog()
    {
        System.Console.WriteLine(" Product Catalog \n");

        System.Console.WriteLine("Individual Products:");
        foreach (var product in products)
        {
            product.Display();
        }

        System.Console.WriteLine("\nProduct Bundles:");
        foreach (var bundle in bundles)
        {
            bundle.Display();
            System.Console.WriteLine();
        }
    }

    private void InitializeInventory()
    {
        System.Console.WriteLine(" Initializing Inventory \n");

        _inventorySystem.AddStock(keyboard.Id, 10);
        _inventorySystem.AddStock(mouse.Id, 15);
        _inventorySystem.AddStock(webcam.Id, 8);
        _inventorySystem.AddStock(headset.Id, 5);
        _inventorySystem.AddStock(monitor.Id, 3);
        _inventorySystem.AddStock(deskLamp.Id, 12);

        System.Console.WriteLine("Inventory initialized\n");
    }

    private void DisplayInventory()
    {
        System.Console.WriteLine(" Current Inventory \n");

        foreach (var product in products)
        {
            var quantity = _inventorySystem.GetAvailableQuantity(product.Id);
            System.Console.WriteLine($"{product.Name} ({product.Id}): {quantity} units");
        }

        System.Console.WriteLine();
    }

    private void ProcessOrders()
    {
        System.Console.WriteLine(" Processing Orders \n");

        ProcessOrder1();
        ProcessOrder2();
        ProcessOrder3();
    }

    private void ProcessOrder1()
    {
        var order = new Order("Kanye West");
        order.AddItem(new OrderItem(keyboard, 2));
        order.AddItem(new OrderItem(mouse, 2));

        order.Display();
        System.Console.WriteLine();

        var paymentGateway = new PayPalGateway();
        var notificationSender = new EmailSender();

        _checkoutFacade.PlaceOrder(order, paymentGateway, notificationSender);
    }

    private void ProcessOrder2()
    {
        var order = new Order("Jared Leto");
        order.AddItem(new OrderItem(proBundle, 1));

        order.Display();
        System.Console.WriteLine();

        var paymentGateway = new StripeGateway();
        var notificationSender = new SmsSender();

        _checkoutFacade.PlaceOrder(order, paymentGateway, notificationSender);
    }

    private void ProcessOrder3()
    {
        var order = new Order("Marcel Bostan");
        order.AddItem(new OrderItem(ultraBundle, 1));
        order.AddItem(new OrderItem(headset, 2));

        order.Display();
        System.Console.WriteLine();

        var paymentGateway = new PayPalGateway();
        var notificationSender = new PushNotificationSender();

        _checkoutFacade.PlaceOrder(order, paymentGateway, notificationSender);
    }

    private void DisplayFinalInventory()
    {
        System.Console.WriteLine(" Final Inventory Status \n");

        foreach (var product in products)
        {
            var quantity = _inventorySystem.GetAvailableQuantity(product.Id);
            System.Console.WriteLine($"{product.Name} ({product.Id}): {quantity} units remaining");
        }
    }
}