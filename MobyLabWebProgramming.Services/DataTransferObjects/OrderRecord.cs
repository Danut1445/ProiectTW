using MobyLabWebProgramming.Database.Repository.Enums;
using MobyLabWebProgramming.Database.Repository.Entities;

namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class OrderRecord
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public double TotalPrice { get; set; }
    public OrderStatusEnum OrderStatus { get; set; }
    public int NumberOfOrderItems { get; set; }
}