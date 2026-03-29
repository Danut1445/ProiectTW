namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class ShopItemAddRecord
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
}