using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Specifications;

public class AnimalSpeciesProjectionSpec : Specification<AnimalSpecies, AnimalSpeciesRecord>
{
    public AnimalSpeciesProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(x => x.Specie, orderByCreatedAt)
            .Select(e => new()
            {
                Specie =  e.Specie,
                Description =  e.Description,
                FoodType =  e.FoodType,
            });

    public AnimalSpeciesProjectionSpec(string specie) : this() => Query.Where(e => e.Specie == specie);
}