namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class ShopItemRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int NumberOfUses { get; set; }
}