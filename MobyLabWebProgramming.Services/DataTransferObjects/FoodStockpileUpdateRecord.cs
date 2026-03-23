namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class FoodStockpileUpdateRecord
{
    public Guid UserId { get; set; }
    public int Grains { get; set; }
    public int Meats { get; set; }
    public int FishFood { get; set; }
    public int Plants { get; set; }
    public int Fish { get; set; }
}