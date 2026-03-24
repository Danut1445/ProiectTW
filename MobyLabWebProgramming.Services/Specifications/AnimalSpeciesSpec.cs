using Ardalis.Specification;
using MobyLabWebProgramming.Database.Repository.Entities;

namespace MobyLabWebProgramming.Services.Specifications;

public class AnimalSpeciesSpec : Specification<AnimalSpecies>
{
    public AnimalSpeciesSpec(string specie) => Query.Where(e => e.Specie == specie);
}