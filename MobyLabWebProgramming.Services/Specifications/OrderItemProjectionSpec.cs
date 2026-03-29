using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Specifications;

public class OrderItemProjectionSpec : Specification<OrderItem, OrderItemRecord>
{
    public OrderItemProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(x => x.CreatedAt, orderByCreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                Price = e.Quantity * e.ShopItem.Price,
                Quantity = e.Quantity,
                ShopItemRecord = new ShopItemRecord
                {
                    Description = e.ShopItem.Description,
                    Price = e.ShopItem.Price,
                    Id = e.ShopItem.Id,
                    Name = e.ShopItem.Name
                }
            });
    
    public OrderItemProjectionSpec(Guid id) : this() => Query.Where(e => e.Id == id);
    
    public OrderItemProjectionSpec(string? search, Guid orderId) : this(true) // This constructor will call the first declared constructor with 'true' as the parameter. 
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            Query.Where(e => e.OrderId == orderId);
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.ShopItem.Name, searchExpr));
    }
}