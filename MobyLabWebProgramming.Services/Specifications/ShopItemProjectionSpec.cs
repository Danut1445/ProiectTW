using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Specifications;

public class ShopItemProjectionSpec : Specification<ShopItem, ShopItemRecord>
{
    public ShopItemProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(x => x.CreatedAt, orderByCreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                Description = e.Description,
                Name = e.Name,
                Price = e.Price,
            });
    
    public ShopItemProjectionSpec(Guid id) : this() => Query.Where(e => e.Id == id);
}