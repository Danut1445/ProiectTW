using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class OrderUpdateRecord
{
    public Guid Id { get; set; }
    public OrderStatusEnum Status { get; set; }
}