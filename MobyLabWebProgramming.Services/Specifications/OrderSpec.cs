using Ardalis.Specification;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Services.Specifications;

public class OrderSpec : Specification<Order>
{
    public OrderSpec(Guid id) => Query.Where(e => e.Id == id);
    
    public OrderSpec(Guid userId, OrderStatusEnum status) => Query.Where(e => e.UserId == userId && e.OrderStatus == status);
}