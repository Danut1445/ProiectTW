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
    
    public AnimalSpeciesProjectionSpec(string? search, bool notImportant) : this(true) // This constructor will call the first declared constructor with 'true' as the parameter. 
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.Specie, searchExpr)); // This is an example on how database specific expressions can be used via C# expressions.
        // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }
}