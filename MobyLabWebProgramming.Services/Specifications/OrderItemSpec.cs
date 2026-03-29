using Ardalis.Specification;
using MobyLabWebProgramming.Database.Repository.Entities;

namespace MobyLabWebProgramming.Services.Specifications;

public class OrderItemSpec :  Specification<OrderItem>
{
    public OrderItemSpec(Guid id) => Query.Where(e => e.Id == id);
}