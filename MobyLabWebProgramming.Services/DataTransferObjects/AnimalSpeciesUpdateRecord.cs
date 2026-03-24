using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class AnimalSpeciesUpdateRecord
{
    public string Specie { get; set; } = null!;
    public string Description { get; set; } = null!;
    public FoodTypesEnum FoodType { get; set; }
}