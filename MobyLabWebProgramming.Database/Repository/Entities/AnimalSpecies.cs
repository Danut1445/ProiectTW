using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Database.Repository.Entities;

public class AnimalSpecies
{
    public string Specie { get; set; } = null!;
    public string Description { get; set; } = null!;
    public FoodTypesEnum FoodType { get; set; }

    public IEnumerable<Animal> Animals { get; set; } = null!;
}