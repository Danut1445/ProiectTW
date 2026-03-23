using Ardalis.Specification;
using MobyLabWebProgramming.Database.Repository.Entities;

namespace MobyLabWebProgramming.Services.Specifications;

public class FoodStockpileSpec : Specification<FoodStockpile>
{
    public FoodStockpileSpec(Guid id) => Query.Where(e => e.Id == id);
}