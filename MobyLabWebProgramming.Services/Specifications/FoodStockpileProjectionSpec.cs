using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Specifications;

public class FoodStockpileProjectionSpec : Specification<FoodStockpile, FoodStockpileRecord>
{
    public FoodStockpileProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(x => x.CreatedAt, orderByCreatedAt)
            .Select(e => new()
            {
                Meats =  e.Meats,
                FishFood = e.FishFood,
                Fish = e.Fish,
                Plants = e.Plants,
                Grains = e.Grains
            });

    public FoodStockpileProjectionSpec(Guid id) : this() => Query.Where(e => e.UserId == id);
}