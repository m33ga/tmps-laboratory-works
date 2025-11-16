namespace ECommerceSystem.Domain.Products;

public class ProductBundle : IProductComponent
{
    private static int _idCounter = 1;

    public string Id { get; }
    public string Name { get; }
    private readonly List<IProductComponent> _components;

    public ProductBundle(string name)
    {
        Id = $"BUN{_idCounter:D3}";
        _idCounter++;
        Name = name;
        _components = new List<IProductComponent>();
    }

    public void Add(IProductComponent component)
    {
        _components.Add(component);
    }

    public void Remove(IProductComponent component)
    {
        _components.Remove(component);
    }

    public string GetId() => Id;
    public string GetName() => Name;

    public decimal GetPrice()
    {
        return _components.Sum(c => c.GetPrice());
    }

    public void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}{Name} (Bundle) - ${GetPrice()}");
        foreach (var component in _components)
        {
            component.Display(depth + 1);
        }
    }

    public IReadOnlyList<IProductComponent> GetComponents() => _components.AsReadOnly();
}