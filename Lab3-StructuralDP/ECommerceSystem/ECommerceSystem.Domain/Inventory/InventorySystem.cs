namespace ECommerceSystem.Domain.Inventory;

public class InventorySystem : IInventorySystem
{
    private readonly Dictionary<string, StockItem> _stock;

    public InventorySystem()
    {
        _stock = new Dictionary<string, StockItem>();
    }

    public void AddStock(string productId, int quantity)
    {
        if (_stock.ContainsKey(productId))
        {
            _stock[productId].AddStock(quantity);
        }
        else
        {
            _stock[productId] = new StockItem(productId, quantity);
        }
    }

    public bool CheckAvailability(string productId, int quantity)
    {
        if (!_stock.ContainsKey(productId))
        {
            return false;
        }
        return _stock[productId].HasAvailableStock(quantity);
    }

    public void ReserveStock(string productId, int quantity)
    {
        if (!_stock.ContainsKey(productId))
        {
            throw new InvalidOperationException($"Product {productId} not found in inventory");
        }
        _stock[productId].ReduceStock(quantity);
    }

    public int GetAvailableQuantity(string productId)
    {
        if (!_stock.ContainsKey(productId))
        {
            return 0;
        }
        return _stock[productId].QuantityAvailable;
    }
}