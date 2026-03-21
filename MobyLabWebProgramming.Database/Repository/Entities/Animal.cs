using MobyLabWebProgramming.Infrastructure.BaseObjects;

namespace MobyLabWebProgramming.Database.Repository.Entities;

public class Animal : BaseEntity
{
    public string Name { get; set; } = null!;
    public int Happiness { get; set; }
    public int Hunger { get; set; }
    
    public AnimalSpecies AnimalSpecie { get; set; } = null!;
    public string BornSpecie { get; set; } = null!;
    
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
}