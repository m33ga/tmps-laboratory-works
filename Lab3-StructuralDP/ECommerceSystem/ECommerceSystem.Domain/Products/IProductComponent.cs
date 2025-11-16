namespace ECommerceSystem.Domain.Products;

public interface IProductComponent
{
    string GetId();
    string GetName();
    decimal GetPrice();
    void Display(int depth = 0);
}