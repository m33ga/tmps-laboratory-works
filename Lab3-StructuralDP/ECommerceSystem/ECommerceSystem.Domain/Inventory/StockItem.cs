namespace ECommerceSystem.Domain.Inventory;

public class StockItem
{
    public string ProductId { get; }
    public int QuantityAvailable { get; private set; }

    public StockItem(string productId, int initialQuantity)
    {
        ProductId = productId;
        QuantityAvailable = initialQuantity;
    }

    public bool HasAvailableStock(int quantity)
    {
        return QuantityAvailable >= quantity;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity > QuantityAvailable)
        {
            throw new InvalidOperationException($"Insufficient stock for product {ProductId}");
        }
        QuantityAvailable -= quantity;
    }

    public void AddStock(int quantity)
    {
        QuantityAvailable += quantity;
    }
}