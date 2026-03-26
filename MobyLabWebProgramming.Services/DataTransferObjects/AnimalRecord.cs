namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class AnimalRecord
{
    public Guid UserId { get; set; }
    public Guid AnimalId { get; set; }
    public string Name { get; set; } = null!;
    public int Happiness { get; set; }
    public int Hunger { get; set; }
    public string BornSpecie { get; set; } = null!;
}