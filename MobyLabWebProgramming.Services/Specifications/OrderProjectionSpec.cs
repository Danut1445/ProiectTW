using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Specifications;

public class OrderProjectionSpec : Specification<Order, OrderRecord>
{
    /// <summary>
    /// In this constructor is the projection/mapping expression used to get UserRecord object directly from the database.
    /// </summary>
    public OrderProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(x => x.CreatedAt, orderByCreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                UserId = e.UserId,
                TotalPrice = e.TotalPaid,
                OrderStatus = e.OrderStatus,
                OrderItems = e.OrderItems
            });

    public OrderProjectionSpec(Guid id) : this() => Query.Where(e => e.Id == id); // This constructor will call the first declared constructor with the default parameter. 
    
    public OrderProjectionSpec(string? search, Guid userId) : this(true) // This constructor will call the first declared constructor with 'true' as the parameter. 
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            Query.Where(e => e.UserId == userId);
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => e.UserId == userId);
    }
}