namespace ECommerceSystem.Domain.Inventory;

public interface IInventorySystem
{
    void AddStock(string productId, int quantity);
    bool CheckAvailability(string productId, int quantity);
    void ReserveStock(string productId, int quantity);
    int GetAvailableQuantity(string productId);
}