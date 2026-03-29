using Ardalis.Specification;
using MobyLabWebProgramming.Database.Repository.Entities;

namespace MobyLabWebProgramming.Services.Specifications;

public class ShopItemSpec : Specification<ShopItem>
{
    public ShopItemSpec(Guid id) => Query.Where(e => e.Id == id);
}