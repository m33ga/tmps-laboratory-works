namespace ECommerceSystem.Domain.Products;

public class Product : IProductComponent
{
    private static int _idCounter = 1;

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

    public string GetId() => Id;
    public string GetName() => Name;
    public decimal GetPrice() => Price;

    public void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}{Name} - ${Price}");
    }
}