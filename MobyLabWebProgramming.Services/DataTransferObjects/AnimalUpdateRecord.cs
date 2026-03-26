namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class AnimalUpdateRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}