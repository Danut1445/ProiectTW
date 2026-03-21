using MobyLabWebProgramming.Infrastructure.BaseObjects;

namespace MobyLabWebProgramming.Database.Repository.Entities;

public class FoodStockpile : BaseEntity
{
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public int Grains { get; set; }
    public int Meats { get; set; }
    public int FishFood { get; set; }
    public int Plants { get; set; }
    public int Fish { get; set; }
}