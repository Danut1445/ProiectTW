using MobyLabWebProgramming.Infrastructure.BaseObjects;
using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Database.Repository.Entities;

public class Order : BaseEntity
{
    public double TotalPaid { get; set; }
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = null!;
    public OrderStatusEnum OrderStatus { get; set; }
}