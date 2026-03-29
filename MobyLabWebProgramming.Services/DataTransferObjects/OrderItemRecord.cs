namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class OrderItemRecord
{
    public Guid Id { get; set; }
    public ShopItemRecord ShopItemRecord { get; set; } = null!;
    public int Quantity { get; set; }
    public double Price { get; set; }
}