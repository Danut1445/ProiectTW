namespace MobyLabWebProgramming.Database.Repository.Entities;

public class OrderItem
{
    public Order Order { get; set; } = null!;
    public Guid OrderId { get; set; }
    public ShopItem ShopItem { get; set; } = null!;
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}