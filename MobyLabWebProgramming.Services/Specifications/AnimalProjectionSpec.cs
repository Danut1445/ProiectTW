using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Specifications;

public class AnimalProjectionSpec : Specification<Animal, AnimalRecord>
{
    public AnimalProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(x => x.CreatedAt, orderByCreatedAt)
            .Select(e => new()
            {
                UserId =  e.UserId,
                Name = e.Name,
                BornSpecie = e.BornSpecie,
                Happiness =  e.Happiness,
                Hunger =  e.Hunger
            });
    
    public AnimalProjectionSpec(Guid id) : this() => Query.Where(e => e.Id == id);

    public AnimalProjectionSpec(string? search, Guid userId) : this(true) // This constructor will call the first declared constructor with 'true' as the parameter. 
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            Query.Where(e => e.UserId == userId);
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.Name, searchExpr) && e.UserId == userId); // This is an example on how database specific expressions can be used via C# expressions.
        // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }
}