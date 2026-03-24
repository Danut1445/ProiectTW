using MobyLabWebProgramming.Database.Repository.Enums;
using MobyLabWebProgramming.Infrastructure.BaseObjects;

namespace MobyLabWebProgramming.Database.Repository.Entities;

public class AnimalSpecies : BaseEntity
{
    public string Specie { get; set; } = null!;
    public string Description { get; set; } = null!;
    public FoodTypesEnum FoodType { get; set; }

    public IEnumerable<Animal> Animals { get; set; } = null!;
}